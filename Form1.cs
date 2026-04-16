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
        // 파일 및 폴더 정보를 저장할 딕셔너리 (과제 4: 폴더/파일 통합 관리)
        private Dictionary<string, FileSystemInfo> leftItems = new Dictionary<string, FileSystemInfo>();
        private Dictionary<string, FileSystemInfo> rightItems = new Dictionary<string, FileSystemInfo>();

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
            if (lv == lvwLeftDir) leftItems.Clear();
            else if (lv == lvwRightDir) rightItems.Clear();

            try
            {
                // [과제 4] 폴더와 파일을 하나의 FileSystemInfo로 처리
                DirectoryInfo root = new DirectoryInfo(folderPath);
                var items = root.GetFileSystemInfos().OrderBy(i => i.Name);

                foreach (var item in items)
                {
                    ListViewItem lvItem = new ListViewItem(item.Name);

                    if (item is DirectoryInfo d)
                    {
                        lvItem.SubItems.Add("<DIR>");
                        lvItem.SubItems.Add(d.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (lv == lvwLeftDir) leftItems[item.Name] = d;
                        else rightItems[item.Name] = d;
                    }
                    else if (item is FileInfo f)
                    {
                        lvItem.SubItems.Add(f.Length.ToString("N0") + " 바이트");
                        lvItem.SubItems.Add(f.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (lv == lvwLeftDir) leftItems[item.Name] = f;
                        else rightItems[item.Name] = f;
                    }
                    lv.Items.Add(lvItem);
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
                // [과제 4] 폴더/파일 비교 로직
                ListViewItem rightItem = FindItem(lvwRightDir, leftItem.Text);

                if (rightItem != null) // 양쪽에 파일/폴더가 존재하는 경우
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
                    }
                }
                else // 왼쪽에만 존재하는 단독 파일/폴더 (보라색)
                {
                    leftItem.ForeColor = Color.Purple;
                }
            }

            // 2. 오른쪽 리스트뷰에만 있는 단독 파일/폴더 처리
            foreach (ListViewItem rightItem in lvwRightDir.Items)
            {
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

        // [과제 3/4] 왼쪽에서 오른쪽으로 재귀 복사 (>>> 버튼)
        private void btnCopyFromLeft_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvwLeftDir.SelectedItems)
            {
                if (!leftItems.TryGetValue(item.Text, out var src)) continue;
                // 재귀 복사 및 덮어쓰기 확인 함수 호출
                CopyRecursiveAndConfirm(src, Path.Combine(txtRightDir.Text, src.Name));
            }
            // 복사 후 오른쪽 리스트 갱신
            PopulateListView(lvwRightDir, txtRightDir.Text);
        }

        // [과제 3/4] 오른쪽에서 왼쪽으로 재귀 복사 (<<< 버튼)
        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvwRightDir.SelectedItems)
            {
                if (!rightItems.TryGetValue(item.Text, out var src)) continue;
                // 재귀 복사 및 덮어쓰기 확인 함수 호출
                CopyRecursiveAndConfirm(src, Path.Combine(txtLeftDir.Text, src.Name));
            }
            // 복사 완료 후 왼쪽 리스트 갱신
            PopulateListView(lvwLeftDir, txtLeftDir.Text);
        }

        // [과제 4] 재귀적으로 탐색하며 덮어쓰기 확인 및 복사 수행
        private void CopyRecursiveAndConfirm(FileSystemInfo src, string destPath)
        {
            if (src is FileInfo file)
            {
                // 파일인 경우 덮어쓰기 확인 함수 사용
                CopyFileWithConfirmation(file.FullName, destPath);
            }
            else if (src is DirectoryInfo dir)
            {
                // 폴더인 경우, 폴더는 생성하고 내부 아이템들을 다시 확인
                if (!Directory.Exists(destPath)) Directory.CreateDirectory(destPath);

                foreach (FileSystemInfo subItem in dir.GetFileSystemInfos())
                {
                    CopyRecursiveAndConfirm(subItem, Path.Combine(destPath, subItem.Name));
                }
                // 폴더 수정일 동기화
                new DirectoryInfo(destPath).LastWriteTime = dir.LastWriteTime;
            }
        }

        // [과제 3/4] 덮어쓰기 확인 및 파일 복사 + 수정일 동기화 함수
        // 덮어쓰기 확인 및 복사 수행 함수 (경로 표시 기능 추가)
        private bool CopyFileWithConfirmation(string sourcePath, string destPath)
        {
            if (File.Exists(destPath))
            {
                // 메시지 박스에 원본(sourcePath)과 대상(destPath) 경로를 줄바꿈(\n)으로 추가합니다.
                string message = $"파일 '{Path.GetFileName(destPath)}'이(가) 이미 존재합니다.\n\n" +
                                 $"원본: {sourcePath}\n" +
                                 $"대상: {destPath}\n\n" +
                                 "덮어쓰시겠습니까?";

                var result = MessageBox.Show(message, "덮어쓰기 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return false; // 사용자가 '아니요'를 누르면 복사 중단
            }

            try
            {
                // 파일 복사 수행 (덮어쓰기 허용: true)
                File.Copy(sourcePath, destPath, true);

                // [중요] 수정일 동기화 (원본 파일의 마지막 수정 시간을 대상 파일에 적용)
                FileInfo srcInfo = new FileInfo(sourcePath);
                FileInfo destInfo = new FileInfo(destPath);
                destInfo.LastWriteTime = srcInfo.LastWriteTime;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("복사 중 오류 발생: " + ex.Message);
                return false;
            }
        }

        private void btnLeftDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요.";
                if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text)) dlg.SelectedPath = txtLeftDir.Text;
                if (dlg.ShowDialog() == DialogResult.OK) { txtLeftDir.Text = dlg.SelectedPath; PopulateListView(lvwLeftDir, dlg.SelectedPath); }
            }
        }

        private void btnRightDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요.";
                if (!string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text)) dlg.SelectedPath = txtRightDir.Text;
                if (dlg.ShowDialog() == DialogResult.OK) { txtRightDir.Text = dlg.SelectedPath; PopulateListView(lvwRightDir, dlg.SelectedPath); }
            }
        }
    }
}