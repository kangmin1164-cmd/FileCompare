using System;
using System.Collections.Generic; // Dictionary 사용을 위해 추가
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace FileCompare
{
    public partial class Form1 : Form
    {
        // 파일 정보를 저장할 딕셔너리 (과제 3 복사 기능을 위해 추가)
        private Dictionary<string, FileInfo> leftFiles = new Dictionary<string, FileInfo>();
        private Dictionary<string, FileInfo> rightFiles = new Dictionary<string, FileInfo>();

        public Form1()
        {
            InitializeComponent();
        }

        private void PopulateListView(ListView lv, string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) return;

            lv.BeginUpdate();
            lv.Items.Clear();

            // 리스트를 새로 고칠 때 해당 쪽의 딕셔너리도 초기화
            if (lv == lvwLeftDir) leftFiles.Clear();
            else if (lv == lvwRightDir) rightFiles.Clear();

            try
            { // 폴더(디렉터리) 먼저 추가
                var dirs = Directory.EnumerateDirectories(folderPath)
                    .Select(p => new DirectoryInfo(p))
                    .OrderBy(d => d.Name);
                foreach (var d in dirs)
                {
                    var item = new ListViewItem(d.Name);
                    item.SubItems.Add("<DIR>");
                    item.SubItems.Add(d.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    lv.Items.Add(item);
                }

                // 파일 추가
                var files = Directory.EnumerateFiles(folderPath)
                    .Select(p => new FileInfo(p))
                    .OrderBy(f => f.Name);

                foreach (var f in files)
                {
                    var item = new ListViewItem(f.Name);
                    item.SubItems.Add(f.Length.ToString("N0") + " 바이트");
                    item.SubItems.Add(f.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    lv.Items.Add(item);

                    // 딕셔너리에 파일 정보 저장 (복사 시 사용)
                    if (lv == lvwLeftDir) leftFiles[f.Name] = f;
                    else if (lv == lvwRightDir) rightFiles[f.Name] = f;
                }
                // 컬럼 너비 자동 조정(컨텐츠 기준)
                for (int i = 0; i < lv.Columns.Count; i++)
                {
                    lv.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show(this, "폴더를 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, "입출력 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lv.EndUpdate();
            }
            CompareFiles(); // 목록을 다 불러온 후 양쪽 리스트뷰를 비교하여 색상을 입힙니다.
        }

        // 양쪽 리스트뷰 비교 및 색상 표시 함수
        private void CompareFiles()
        {
            // 두 리스트뷰가 모두 채워졌을 때만 비교를 수행하도록 안전 장치 추가
            if (lvwLeftDir.Items.Count == 0 || lvwRightDir.Items.Count == 0) return;

            // 양쪽 아이템의 색상을 일단 검정색으로 초기화
            foreach (ListViewItem item in lvwLeftDir.Items) item.ForeColor = Color.Black;
            foreach (ListViewItem item in lvwRightDir.Items) item.ForeColor = Color.Black;

            // 1. 왼쪽 리스트뷰 아이템들을 순회하며 오른쪽과 비교
            foreach (ListViewItem leftItem in lvwLeftDir.Items)
            {
                if (leftItem.SubItems[1].Text == "<DIR>") continue;

                ListViewItem rightItem = FindItem(lvwRightDir, leftItem.Text);

                if (rightItem != null) // 양쪽에 파일이 존재하는 경우
                {
                    // Parse 시 발생할 수 있는 공백 및 형식 오류를 방지
                    if (DateTime.TryParse(leftItem.SubItems[2].Text, out DateTime leftDate) &&
                        DateTime.TryParse(rightItem.SubItems[2].Text, out DateTime rightDate))
                    {
                        if (leftDate > rightDate) // 왼쪽이 최신 (New:빨간색, Old:회색)
                        {
                            leftItem.ForeColor = Color.Red;
                            rightItem.ForeColor = Color.Gray;
                        }
                        else if (leftDate < rightDate) // 오른쪽이 최신
                        {
                            leftItem.ForeColor = Color.Gray;
                            rightItem.ForeColor = Color.Red;
                        }
                        else
                        {
                            // 날짜가 완전히 같으면 검정색
                            leftItem.ForeColor = Color.Black;
                            rightItem.ForeColor = Color.Black;
                        }
                    }
                }
                else // 왼쪽에만 존재하는 단독 파일 (보라색)
                {
                    leftItem.ForeColor = Color.Purple;
                }
            }

            // 2. 오른쪽 리스트뷰에만 있는 단독 파일 처리
            foreach (ListViewItem rightItem in lvwRightDir.Items)
            {
                if (rightItem.SubItems[1].Text == "<DIR>") continue;
                if (FindItem(lvwLeftDir, rightItem.Text) == null)
                {
                    rightItem.ForeColor = Color.Purple;
                }
            }
        }

        // 특정 리스트뷰에서 이름으로 아이템을 찾는 보조 함수
        private ListViewItem FindItem(ListView lv, string name)
        {
            foreach (ListViewItem item in lv.Items)
            {
                if (item.Text == name) return item;
            }
            return null;
        }

        // 과제 3: 왼쪽에서 오른쪽으로 복사 (>>> 버튼)
        private void btnCopyFromLeft_Click(object sender, EventArgs e)
        {
            var selected = lvwLeftDir.SelectedItems;

            foreach (ListViewItem item in selected)
            {
                var name = item.Text;
                if (item.SubItems[1].Text == "<DIR>") continue;

                if (!leftFiles.TryGetValue(name, out var src)) continue;

                var destPath = Path.Combine(txtRightDir.Text, src.Name);

                if (CopyFileWithConfirmation(src.FullName, destPath))
                {
                    PopulateListView(lvwRightDir, txtRightDir.Text);
                }
            }
        }

        // [추가] 과제 3: 오른쪽에서 왼쪽으로 복사 (<<< 버튼)
        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
            var selected = lvwRightDir.SelectedItems;

            foreach (ListViewItem item in selected)
            {
                var name = item.Text;
                if (item.SubItems[1].Text == "<DIR>") continue;

                // 오른쪽 딕셔너리에서 파일 정보 찾기
                if (!rightFiles.TryGetValue(name, out var src)) continue;

                var destPath = Path.Combine(txtLeftDir.Text, src.Name);

                // 복사 확인 및 실행
                if (CopyFileWithConfirmation(src.FullName, destPath))
                {
                    // 복사 완료 후 왼쪽 리스트 갱신
                    PopulateListView(lvwLeftDir, txtLeftDir.Text);
                }
            }
        }

        // 과제 3: 덮어쓰기 확인 및 복사 수행 함수
        private bool CopyFileWithConfirmation(string sourcePath, string destPath)
        {
            if (File.Exists(destPath))
            {
                var result = MessageBox.Show($"파일 '{Path.GetFileName(destPath)}'이(가) 이미 존재합니다. 덮어쓸까요?",
                                             "파일 복사 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return false; // 복사 안 함
            }

            try
            {
                File.Copy(sourcePath, destPath, true);
                return true; // 성공 시 true 반환
            }
            catch (Exception ex)
            {
                MessageBox.Show("복사 중 오류 발생: " + ex.Message);
                return false; // 실패 시 false 반환
            }
        }

        private void btnLeftDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요.";

                // 현재 텍스트박스에 있는 경로를 초기 선택 폴더로 설정
                if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text))
                {
                    dlg.SelectedPath = txtLeftDir.Text;
                }
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtLeftDir.Text = dlg.SelectedPath;
                    PopulateListView(lvwLeftDir, dlg.SelectedPath);
                }
            }
        }

        private void btnRightDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요.";

                // 현재 텍스트박스에 있는 경로를 초기 선택 폴더로 설정
                if (!string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text))
                {
                    dlg.SelectedPath = txtRightDir.Text;
                }
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtRightDir.Text = dlg.SelectedPath;
                    PopulateListView(lvwRightDir, dlg.SelectedPath);
                }
            }
        }
    }
}