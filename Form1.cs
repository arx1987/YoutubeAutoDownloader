using OpenQA.Selenium;
using System.Collections.Generic;

namespace YoutubeAutoDownloader
{
  public partial class Form1 : Form
  {
    private IWebDriver driver;
    //string url;
    //private readonly By _path = By.XPath("//*[@id=\"window_register\"]/a");
    public string folderForSaving = "D:\\Programming";
    public Form1()
    {
      InitializeComponent();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
      {
        folderForSaving = folderBrowserDialog1.SelectedPath;
        //---> you should add saving this path to txt file and checking this txt file before opening
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (textBox1.Text == "")
      {
        textBox1.Text = "Please, add youtube html page address to download a playlist";
        textBox1.ForeColor = Color.Red;
      }
      else if (textBox1.Text == "Please, add youtube html page address to download a playlist")
      {
        textBox1.Text = "";
      }
      else//---> add one more check -> is it existing hmtl path(idk how yet)
      {
        driver = new OpenQA.Selenium.Chrome.ChromeDriver();
        driver.Navigate().GoToUrl(textBox1.Text);
        driver.Manage().Window.Maximize();
        Thread.Sleep(1000);
        string url1 = "/html/body/ytd-app/div[1]/ytd-page-manager/ytd-watch-flexy/div[5]/div[2]/div/ytd-playlist-panel-renderer/div/div[3]/ytd-playlist-panel-video-renderer";
        //string url2 = "/html/body/ytd-app/div[1]/ytd-page-manager/ytd-watch-flexy/div[5]/div[2]/div/ytd-playlist-panel-renderer/div/div[3]/ytd-playlist-panel-video-renderer[1]/a";
        int numberOfElements = this.driver.FindElements(By.XPath(url1)).Count();
        if (numberOfElements > 0)
        {
          string[] linksToAllMovies = new string[numberOfElements];
          for (int i = 0; i < numberOfElements; i++)
          {
            //get url
            int j = i + 1;
            string href = this.driver.FindElement(By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-watch-flexy/div[5]/div[2]/div/ytd-playlist-panel-renderer/div/div[3]/ytd-playlist-panel-video-renderer[" + j + "]/a")).GetAttribute("href");
            //change url
            linksToAllMovies[i] = href.Insert(12, "ss");
          }
          for (int i = 0; i < numberOfElements; i++)
          {
            driver.Navigate().GoToUrl(linksToAllMovies[i]);
            //wait 10 sec to fully apdate the page   (---> find a better way!)
            Thread.Sleep(10000);
            //find donwload button and click it
            driver.FindElement(By.XPath("/html/body/div[2]/div[1]/div[3]/div[4]/div/div[1]/div[2]/div[2]/div[1]/a")).Click();
          }
        }
        //---> close the window in 1 min
        Thread.Sleep(60000);
        Console.WriteLine("The end?");
      }
    }
  }
}