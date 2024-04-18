using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace 학교_주소_추출
{
    public partial class Form1 : Form
    {
        private HttpClient httpClient;

        public Form1()
        {
            InitializeComponent();
            httpClient = new HttpClient();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            label5.Text = "검색중..."; // 검색이 시작될 때 라벨 텍스트 변경

            string[] searchKeywords = textBox1.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                List<string> searchResults1 = new List<string>();
                List<string> searchResults2 = new List<string>();
                List<string> searchResults3 = new List<string>();

                foreach (string keyword in searchKeywords)
                {
                    string url = "https://search.naver.com/search.naver?ie=UTF-8&query=" + Uri.EscapeDataString(keyword);
                    string html = await httpClient.GetStringAsync(url);
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);

                    var element1 = doc.DocumentNode.SelectSingleNode("//*[@id='main_pack']/div[3]/div[2]/div[1]/div/div[2]/dl/div[2]/dd");
                    var element2 = doc.DocumentNode.SelectSingleNode("//*[@id='main_pack']/div[3]/div[1]/div[1]/div/span[1]");
                    var element3 = doc.DocumentNode.SelectSingleNode("//*[@id='main_pack']/div[3]/div[2]/div[1]/div/div[2]/dl/div[1]/dd");

                    if (element1 != null)
                    {
                        searchResults1.Add(element1.InnerText);
                    }
                    else
                    {
                        searchResults1.Add("결과를 찾을 수 없습니다.");
                    }

                    if (element2 != null)
                    {
                        searchResults2.Add(element2.InnerText);
                    }
                    else
                    {
                        searchResults2.Add("결과를 찾을 수 없습니다.");
                    }

                    if (element3 != null)
                    {
                        searchResults3.Add(element3.InnerText);
                    }
                    else
                    {
                        searchResults3.Add("결과를 찾을 수 없습니다.");
                    }
                }

                textBox3.Text = string.Join(Environment.NewLine, searchResults1);
                textBox4.Text = string.Join(Environment.NewLine, searchResults2);
                textBox2.Text = string.Join(Environment.NewLine, searchResults3);

                label5.Text = "검색 완료"; // 검색이 완료되면 라벨 텍스트 변경
            }
            catch (Exception ex)
            {
                label5.Text = "에러 발생: " + ex.Message; // 에러가 발생하면 라벨 텍스트 변경
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            httpClient?.Dispose();  // HttpClient 종료
        }
    }
}
