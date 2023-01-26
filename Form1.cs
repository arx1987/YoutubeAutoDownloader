using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;

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
        textBox1.ForeColor = Color.Black;
      }
      else//---> add one more check -> is it existing hmtl path(idk how yet)
      {
        //create an default option for chromebrowser. It sets default download folder 
        ChromeOptions chromeOptions = new ChromeOptions();
        chromeOptions.AddUserProfilePreference("download.default_directory", folderForSaving);
        driver = new OpenQA.Selenium.Chrome.ChromeDriver(chromeOptions);
        driver.Navigate().GoToUrl(textBox1.Text);
        driver.Manage().Window.Maximize();
        Thread.Sleep(1000);
        //find all playlist's movies
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
            int j = i + 1;
            driver.Navigate().GoToUrl(linksToAllMovies[i]);
            //wait 10 sec to fully apdate the page   (---> find a better way!)
            Thread.Sleep(10000);
            /*  //Check it!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             * // Wait until a page is fully loaded via JavaScript
            WebDriverWait wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(30));
            wait.Until((x) =>
            {
              return ((IJavaScriptExecutor)this.driver).ExecuteScript("return document.readyState").Equals("complete");
            });
             */
            //find donwload button and click it
            driver.FindElement(By.XPath("/html/body/div[2]/div[1]/div[3]/div[4]/div/div[1]/div[2]/div[2]/div[1]/a")).Click();
            //OpenQA.Selenium.NoSuchElementException
            //Get download link
            //string fileName = driver.FindElement(By.XPath("/html/body/div[2]/div[1]/div[3]/div[4]/div/div[1]/div[2]/div[2]/div[1]/a")).GetAttribute("download");
            //string downloadHref = driver.FindElement(By.XPath("/html/body/div[2]/div[1]/div[3]/div[4]/div/div[1]/div[2]/div[2]/div[1]/a")).GetAttribute("href");
          }
        }
        //---> close the window in 1 min
        Thread.Sleep(60000);
        Console.WriteLine("The end?");
        driver.Quit();
      }
    }
  }
}