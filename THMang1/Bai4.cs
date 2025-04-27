using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using THMang1;
using HtmlAgilityPack;

namespace THMang1
{
    public partial class Bai4 : Form
    {
        public Bai4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }

            try
            {
                webBrowser.Navigate(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải trang: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }

            // Mở FolderDialog để chọn thư mục lưu trữ
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Chọn thư mục để lưu trữ các tài nguyên và HTML";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string downloadFolder = folderDialog.SelectedPath;
                    string htmlFilePath = Path.Combine(downloadFolder, "index.html");

                    try
                    {
                        // Tải HTML trang web
                        HtmlWeb web = new HtmlWeb();
                        HtmlAgilityPack.HtmlDocument doc = web.Load(url);

                        // Lấy HTML của trang
                        string htmlContent = doc.DocumentNode.OuterHtml;

                        // Các tài nguyên cần tải
                        List<string> resourcesToDownload = new List<string>();

                        // Lọc tất cả các hình ảnh (src) và CSS (href) từ HTML
                        var imgNodes = doc.DocumentNode.SelectNodes("//img[@src]");
                        if (imgNodes != null)
                        {
                            foreach (var imgNode in imgNodes)
                            {
                                string imgSrc = imgNode.GetAttributeValue("src", "");
                                if (!string.IsNullOrEmpty(imgSrc) && !imgSrc.StartsWith("http"))
                                {
                                    imgSrc = new Uri(new Uri(url), imgSrc).ToString();
                                }
                                resourcesToDownload.Add(imgSrc);
                            }
                        }

                        var linkNodes = doc.DocumentNode.SelectNodes("//link[@href]");
                        if (linkNodes != null)
                        {
                            foreach (var linkNode in linkNodes)
                            {
                                string linkHref = linkNode.GetAttributeValue("href", "");
                                if (!string.IsNullOrEmpty(linkHref) && !linkHref.StartsWith("http"))
                                {
                                    linkHref = new Uri(new Uri(url), linkHref).ToString();
                                }
                                resourcesToDownload.Add(linkHref);
                            }
                        }

                        var scriptNodes = doc.DocumentNode.SelectNodes("//script[@src]");
                        if (scriptNodes != null)
                        {
                            foreach (var scriptNode in scriptNodes)
                            {
                                string scriptSrc = scriptNode.GetAttributeValue("src", "");
                                if (!string.IsNullOrEmpty(scriptSrc) && !scriptSrc.StartsWith("http"))
                                {
                                    scriptSrc = new Uri(new Uri(url), scriptSrc).ToString();
                                }
                                resourcesToDownload.Add(scriptSrc);
                            }
                        }

                        // Tải các tài nguyên (hình ảnh, JS, CSS)
                        WebClient client = new WebClient();
                        foreach (var resource in resourcesToDownload)
                        {
                            string resourceName = Path.GetFileName(new Uri(resource).LocalPath);
                            string filePath = Path.Combine(downloadFolder, resourceName);
                            try
                            {
                                client.DownloadFile(resource, filePath);
                                htmlContent = htmlContent.Replace(resource, Path.Combine(downloadFolder, resourceName));
                            }
                            catch (Exception)
                            {
                                // Không tải được thì bỏ qua
                            }
                        }

                        // Lưu HTML đã sửa lại với đường dẫn tài nguyên local
                        File.WriteAllText(htmlFilePath, htmlContent, Encoding.UTF8);

                        // Mở trang HTML trong WebBrowser
                        webBrowser.Navigate("file:///" + htmlFilePath);
                        MessageBox.Show("Tải trang và tài nguyên thành công!");

                        // Mở source HTML trong WebSource form
                        WebSource sourceForm = new WebSource(htmlContent);
                        sourceForm.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi tải trang hoặc tài nguyên: " + ex.Message);
                    }
                }
            }
        }
    }
}
