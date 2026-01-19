using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Buffers.Text;
using System.Linq;
using System.Threading;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Front.SeleniumTests
{
    [TestFixture]
    public class AuthTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string BaseUrl = "http://localhost:5041"; // consistent naming

        [SetUp]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            // options.AddArgument("--headless");   // uncomment later when stable

            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(12)); // a bit more patience
        }

       [Test, Order(1)]
        public void Acceder_Directement_A_Livres_Page()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Livres");

            try
            {
                var localWait = new WebDriverWait(driver, TimeSpan.FromSeconds(1500)); // ← monte même à 45–60 s si besoin

                var pageTitle = localWait.Until(ExpectedConditions.ElementIsVisible(By.TagName("h1")));

                // → Ici la page est affichée et le h1 est visible

                Thread.Sleep(1500);     
  

                Assert.That(driver.Url, Does.Contain("/Livres"),
                    "La navigation directe vers /Livres a échché");

                StringAssert.Contains("Livres", pageTitle.Text,
                    "La page /Livres ne contient pas le titre attendu");
            }
            catch (WebDriverTimeoutException ex)
            {
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("acces-livres-failure.png");
                Assert.Fail($"Timeout lors de l'accès à /Livres : {ex.Message}");
            }
        }
        [Test, Order(2)]
        public void Register_NewUser_Success()
        {
            // ────────────────────────────────────────────────────────────────
            // Your original working version – untouched
            // ────────────────────────────────────────────────────────────────
            driver.Navigate().GoToUrl($"{BaseUrl}/register");
            string randomUser = "user" + DateTime.Now.Ticks;

            try
            {
                var username = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("input[placeholder='Choisissez un username']")));
                username.Clear();
                username.SendKeys(randomUser);

                var email = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("input[placeholder='votre@email.com']")));
                email.Clear();
                email.SendKeys($"{randomUser}@test.com");

                var password = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("input[placeholder='Minimum 6 caractères']")));
                password.Clear();
                password.SendKeys("Test123!");

                var confirm = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("input[placeholder='Répétez le mot de passe']")));
                confirm.Clear();
                confirm.SendKeys("Test123!");

                var submit = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("button[type='submit']")));

                Thread.Sleep(600);
                submit.Click();

                wait.Until(d => d.Url.Contains("/login"));
                Assert.That(driver.Url.Contains("/login"), "Redirection vers /login échouée après inscription");
            }
            catch (WebDriverTimeoutException e)
            {
                Assert.Fail("Échec de l'inscription : " + e.Message);
            }
        }
        [Test, Order(3)]
        public void Acceder_A_Details_Livre()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Livres");

            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(300)); // 5 minutes max

                // 1️⃣ Attendre chargement complet Blazor
                wait.Until(d =>
                    ((IJavaScriptExecutor)d)
                        .ExecuteScript("return document.readyState")
                        .Equals("complete")
                );

                Thread.Sleep(3000); // 👀 voir la page Livres

                // 2️⃣ Attendre que les boutons Détails existent
                wait.Until(d =>
                    d.FindElements(By.CssSelector(
                        "a.btn.btn-outline-primary.btn-sm"))
                    .Count > 0
                );

                Thread.Sleep(3000); // 👀 voir les livres affichés

                // 3️⃣ Récupérer le lien Détails
                var detailsLink = driver
                    .FindElements(By.CssSelector("a.btn.btn-outline-primary.btn-sm"))
                    .First()
                    .GetAttribute("href");

                Assert.IsNotNull(detailsLink, "Lien Détails introuvable");

                // 4️⃣ Naviguer vers la page Détails
                driver.Navigate().GoToUrl(detailsLink);

                // 5️⃣ Attendre la page Détails
                wait.Until(d => d.Url.Contains("/Livres/Details/"));

                Thread.Sleep(5000); // 👀 VOIR LA PAGE DÉTAILS

                // 6️⃣ Vérification finale
                Assert.That(driver.Url, Does.Contain("/Livres/Details/"),
                    "La page Détails ne s’est pas affichée");
            }
            catch (WebDriverTimeoutException ex)
            {
                ((ITakesScreenshot)driver)
                    .GetScreenshot()
                    .SaveAsFile("details-timeout.png");

                Assert.Fail($"Timeout page détails : {ex.Message}");
            }
        }





        [TearDown]
        public void Cleanup()
        {
            try { driver?.Quit(); } catch { }
            try { driver?.Dispose(); } catch { }
        }
    }
}