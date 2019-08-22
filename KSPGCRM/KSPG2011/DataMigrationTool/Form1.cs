using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using KSPG.Dal.EntityClasses;
using KSPG.Dal.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace DataMigrationTool
{
    public partial class Form1 : Form
    {
        List<Reporter> _reporters = new List<Reporter>();

        List<ArticleView> _articleViews = new List<ArticleView>();

        List<Author> _authors = new List<Author>();
        public Form1()
        {
            InitializeComponent();
        }

        private void butUpdateDefaultImage_Click(object sender, EventArgs e)
        {




            var thread = new Thread(delegate()
            {
                var md = new LinqMetaData();
                var articles = md.Article.Where(t => t.IsPublished);


                if (progUpdateDefaultImage.InvokeRequired)
                {
                    progUpdateDefaultImage.Invoke(new MethodInvoker(delegate { progUpdateDefaultImage.Maximum = articles.Count(); }));
                }
                else
                {
                    progUpdateDefaultImage.Maximum = articles.Count();
                }
                foreach (ArticleEntity article in articles)
                {

                    var imageGalleries = from t in article.GalleryCollectionViaArticleGallery.Where(t => t.MediaTypeId == 0) select t;
                    if (imageGalleries.Count() > 0)
                    {
                        var medias = imageGalleries.FirstOrDefault().MediaCollectionViaMediaGallery.Where(t => t.IsGalleryCover);
                        var mediaEntities = medias as MediaEntity[] ?? medias.ToArray();
                        if (mediaEntities.Any())
                        {
                            article.DefaultImageId = mediaEntities.ToArray()[0].MediaId;
                            article.Save();
                        }
                    }
                    Invoke((MethodInvoker)delegate
                    {
                        progUpdateDefaultImage.Increment(1);
                        lblProgress.Text = string.Format("{0} of {1}", progUpdateDefaultImage.Value,
                            progUpdateDefaultImage.Maximum);
                    });

                }

            });

            thread.Start();


        }


        public static AnalyticsService AuthenticateServiceAccount(string serviceAccountEmail, string keyFilePath)
        {

            // check the file exists
            if (!File.Exists(keyFilePath))
            {
                Console.WriteLine("An Error occurred - Key file does not exist");
                return null;
            }

            string[] scopes = new string[] { AnalyticsService.Scope.Analytics,  // view and manage your analytics data
                                             AnalyticsService.Scope.AnalyticsEdit,  // edit management actives
                                             AnalyticsService.Scope.AnalyticsManageUsers,   // manage users
                                             AnalyticsService.Scope.AnalyticsReadonly};     // View analytics data            

            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            try
            {
                ServiceAccountCredential credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = scopes
                    }.FromCertificate(certificate));

                // Create the service.
                AnalyticsService service = new AnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Analytics API Sample",
                });
                return service;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.InnerException);
                return null;

            }
        }

        private void InitializeArticleViews()
        {
            var lines = textBox1.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var cols = line.Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                _articleViews.Add(new ArticleView() { ArticleId = int.Parse(cols[0].Replace("/article/", string.Empty).Replace(".html",string.Empty)), View = int.Parse(cols[1]) });
            }
            return;
            _articleViews.Add(new ArticleView() { ArticleId = 109926, View = 6891 });
            _articleViews.Add(new ArticleView() { ArticleId = 109929, View = 692 });
            _articleViews.Add(new ArticleView() { ArticleId = 109958, View = 3950 });
            _articleViews.Add(new ArticleView() { ArticleId = 109960, View = 1018 });
            _articleViews.Add(new ArticleView() { ArticleId = 110064, View = 206 });
            _articleViews.Add(new ArticleView() { ArticleId = 110065, View = 1714 });
            _articleViews.Add(new ArticleView() { ArticleId = 110010, View = 760 });
            _articleViews.Add(new ArticleView() { ArticleId = 110015, View = 9381 });
            _articleViews.Add(new ArticleView() { ArticleId = 110018, View = 880 });
            _articleViews.Add(new ArticleView() { ArticleId = 110134, View = 791 });
            _articleViews.Add(new ArticleView() { ArticleId = 110141, View = 2070 });
            _articleViews.Add(new ArticleView() { ArticleId = 110066, View = 10830 });
            _articleViews.Add(new ArticleView() { ArticleId = 110070, View = 6005 });
            _articleViews.Add(new ArticleView() { ArticleId = 110177, View = 19766 });
            _articleViews.Add(new ArticleView() { ArticleId = 110179, View = 682 });
            _articleViews.Add(new ArticleView() { ArticleId = 110182, View = 276 });
            _articleViews.Add(new ArticleView() { ArticleId = 110022, View = 1319 });
            _articleViews.Add(new ArticleView() { ArticleId = 110025, View = 1028 });
            _articleViews.Add(new ArticleView() { ArticleId = 109939, View = 3141 });
            _articleViews.Add(new ArticleView() { ArticleId = 109944, View = 7176 });
            _articleViews.Add(new ArticleView() { ArticleId = 109973, View = 2509 });
            _articleViews.Add(new ArticleView() { ArticleId = 109985, View = 20218 });
            _articleViews.Add(new ArticleView() { ArticleId = 110071, View = 2884 });
            _articleViews.Add(new ArticleView() { ArticleId = 110072, View = 594 });
            _articleViews.Add(new ArticleView() { ArticleId = 110145, View = 99773 });
            _articleViews.Add(new ArticleView() { ArticleId = 110148, View = 1032 });
            _articleViews.Add(new ArticleView() { ArticleId = 110184, View = 3522 });
            _articleViews.Add(new ArticleView() { ArticleId = 110185, View = 11072 });
            _articleViews.Add(new ArticleView() { ArticleId = 110186, View = 3458 });
            _articleViews.Add(new ArticleView() { ArticleId = 110026, View = 264 });
            _articleViews.Add(new ArticleView() { ArticleId = 110027, View = 2221 });
            _articleViews.Add(new ArticleView() { ArticleId = 110028, View = 8208 });
            _articleViews.Add(new ArticleView() { ArticleId = 109945, View = 719 });
            _articleViews.Add(new ArticleView() { ArticleId = 109946, View = 2663 });
            _articleViews.Add(new ArticleView() { ArticleId = 109948, View = 2259 });
            _articleViews.Add(new ArticleView() { ArticleId = 110073, View = 729 });
            _articleViews.Add(new ArticleView() { ArticleId = 110074, View = 1964 });
            _articleViews.Add(new ArticleView() { ArticleId = 110078, View = 167 });
            _articleViews.Add(new ArticleView() { ArticleId = 110079, View = 127 });
            _articleViews.Add(new ArticleView() { ArticleId = 110084, View = 1863 });
            _articleViews.Add(new ArticleView() { ArticleId = 110089, View = 392 });
            _articleViews.Add(new ArticleView() { ArticleId = 110092, View = 949 });
            _articleViews.Add(new ArticleView() { ArticleId = 110094, View = 210 });
            _articleViews.Add(new ArticleView() { ArticleId = 110190, View = 7975 });
            _articleViews.Add(new ArticleView() { ArticleId = 110191, View = 2697 });
            _articleViews.Add(new ArticleView() { ArticleId = 110193, View = 46 });
            _articleViews.Add(new ArticleView() { ArticleId = 110199, View = 5192 });
            _articleViews.Add(new ArticleView() { ArticleId = 110200, View = 1254 });
            _articleViews.Add(new ArticleView() { ArticleId = 109950, View = 17075 });
            _articleViews.Add(new ArticleView() { ArticleId = 109951, View = 4088 });
            _articleViews.Add(new ArticleView() { ArticleId = 109954, View = 133 });
            _articleViews.Add(new ArticleView() { ArticleId = 109955, View = 1410 });
            _articleViews.Add(new ArticleView() { ArticleId = 110236, View = 407 });
            _articleViews.Add(new ArticleView() { ArticleId = 110237, View = 579 });
            _articleViews.Add(new ArticleView() { ArticleId = 110246, View = 3862 });
            _articleViews.Add(new ArticleView() { ArticleId = 110292, View = 2939 });
            _articleViews.Add(new ArticleView() { ArticleId = 110302, View = 1701 });
            _articleViews.Add(new ArticleView() { ArticleId = 110305, View = 2462 });
            _articleViews.Add(new ArticleView() { ArticleId = 110346, View = 2855 });
            _articleViews.Add(new ArticleView() { ArticleId = 110347, View = 669 });
            _articleViews.Add(new ArticleView() { ArticleId = 110348, View = 6657 });
            _articleViews.Add(new ArticleView() { ArticleId = 110249, View = 1903 });
            _articleViews.Add(new ArticleView() { ArticleId = 110250, View = 1195 });
            _articleViews.Add(new ArticleView() { ArticleId = 110253, View = 980 });
            _articleViews.Add(new ArticleView() { ArticleId = 110254, View = 1119 });
            _articleViews.Add(new ArticleView() { ArticleId = 110095, View = 1038 });
            _articleViews.Add(new ArticleView() { ArticleId = 110097, View = 5989 });
            _articleViews.Add(new ArticleView() { ArticleId = 110103, View = 541 });
            _articleViews.Add(new ArticleView() { ArticleId = 110104, View = 3486 });
            _articleViews.Add(new ArticleView() { ArticleId = 110349, View = 2785 });
            _articleViews.Add(new ArticleView() { ArticleId = 110350, View = 1239 });
            _articleViews.Add(new ArticleView() { ArticleId = 110205, View = 319 });
            _articleViews.Add(new ArticleView() { ArticleId = 110218, View = 14293 });
            _articleViews.Add(new ArticleView() { ArticleId = 110352, View = 1930 });
            _articleViews.Add(new ArticleView() { ArticleId = 110357, View = 2332 });
            _articleViews.Add(new ArticleView() { ArticleId = 110360, View = 1245 });
            _articleViews.Add(new ArticleView() { ArticleId = 110149, View = 2083 });
            _articleViews.Add(new ArticleView() { ArticleId = 110150, View = 1925 });
            _articleViews.Add(new ArticleView() { ArticleId = 110154, View = 2305 });
            _articleViews.Add(new ArticleView() { ArticleId = 109986, View = 831 });
            _articleViews.Add(new ArticleView() { ArticleId = 109989, View = 138 });
            _articleViews.Add(new ArticleView() { ArticleId = 110258, View = 511 });
            _articleViews.Add(new ArticleView() { ArticleId = 110260, View = 117 });
            _articleViews.Add(new ArticleView() { ArticleId = 110261, View = 1598 });
            _articleViews.Add(new ArticleView() { ArticleId = 110106, View = 131 });
            _articleViews.Add(new ArticleView() { ArticleId = 110108, View = 4257 });
            _articleViews.Add(new ArticleView() { ArticleId = 110110, View = 16754 });
            _articleViews.Add(new ArticleView() { ArticleId = 110396, View = 9818 });
            _articleViews.Add(new ArticleView() { ArticleId = 110397, View = 95 });
            _articleViews.Add(new ArticleView() { ArticleId = 110221, View = 378 });
            _articleViews.Add(new ArticleView() { ArticleId = 110224, View = 3200 });
            _articleViews.Add(new ArticleView() { ArticleId = 110230, View = 24436 });
            _articleViews.Add(new ArticleView() { ArticleId = 110155, View = 510 });
            _articleViews.Add(new ArticleView() { ArticleId = 110156, View = 691 });
            _articleViews.Add(new ArticleView() { ArticleId = 110232, View = 7902 });
            _articleViews.Add(new ArticleView() { ArticleId = 110233, View = 148 });
            _articleViews.Add(new ArticleView() { ArticleId = 110398, View = 135 });
            _articleViews.Add(new ArticleView() { ArticleId = 110401, View = 4461 });
            _articleViews.Add(new ArticleView() { ArticleId = 110405, View = 734 });
            _articleViews.Add(new ArticleView() { ArticleId = 109991, View = 143 });
            _articleViews.Add(new ArticleView() { ArticleId = 109994, View = 1542 });
            _articleViews.Add(new ArticleView() { ArticleId = 110407, View = 430 });
            _articleViews.Add(new ArticleView() { ArticleId = 110408, View = 1003 });
            _articleViews.Add(new ArticleView() { ArticleId = 110235, View = 849 });
            _articleViews.Add(new ArticleView() { ArticleId = 110442, View = 1530 });
            _articleViews.Add(new ArticleView() { ArticleId = 110447, View = 20850 });
            _articleViews.Add(new ArticleView() { ArticleId = 110453, View = 1871 });
            _articleViews.Add(new ArticleView() { ArticleId = 110263, View = 1776 });
            _articleViews.Add(new ArticleView() { ArticleId = 110266, View = 1420 });
            _articleViews.Add(new ArticleView() { ArticleId = 110411, View = 227 });
            _articleViews.Add(new ArticleView() { ArticleId = 110414, View = 1185 });
            _articleViews.Add(new ArticleView() { ArticleId = 110267, View = 10487 });
            _articleViews.Add(new ArticleView() { ArticleId = 110268, View = 160 });
            _articleViews.Add(new ArticleView() { ArticleId = 109995, View = 2514 });
            _articleViews.Add(new ArticleView() { ArticleId = 109997, View = 355 });
            _articleViews.Add(new ArticleView() { ArticleId = 109999, View = 1121 });
            _articleViews.Add(new ArticleView() { ArticleId = 110000, View = 4294 });
            _articleViews.Add(new ArticleView() { ArticleId = 110001, View = 3154 });
            _articleViews.Add(new ArticleView() { ArticleId = 110306, View = 1397 });
            _articleViews.Add(new ArticleView() { ArticleId = 110307, View = 4565 });
            _articleViews.Add(new ArticleView() { ArticleId = 110496, View = 1831 });
            _articleViews.Add(new ArticleView() { ArticleId = 110498, View = 1360 });
            _articleViews.Add(new ArticleView() { ArticleId = 110309, View = 6590 });
            _articleViews.Add(new ArticleView() { ArticleId = 110311, View = 1542 });
            _articleViews.Add(new ArticleView() { ArticleId = 110312, View = 4666 });
            _articleViews.Add(new ArticleView() { ArticleId = 110362, View = 693 });
            _articleViews.Add(new ArticleView() { ArticleId = 110367, View = 1300 });
            _articleViews.Add(new ArticleView() { ArticleId = 110501, View = 230 });
            _articleViews.Add(new ArticleView() { ArticleId = 110505, View = 477 });
            _articleViews.Add(new ArticleView() { ArticleId = 110371, View = 4487 });
            _articleViews.Add(new ArticleView() { ArticleId = 110373, View = 2455 });
            _articleViews.Add(new ArticleView() { ArticleId = 110275, View = 19 });
            _articleViews.Add(new ArticleView() { ArticleId = 110277, View = 60171 });
            _articleViews.Add(new ArticleView() { ArticleId = 110280, View = 4584 });
            _articleViews.Add(new ArticleView() { ArticleId = 110376, View = 1186 });
            _articleViews.Add(new ArticleView() { ArticleId = 110378, View = 1100 });
            _articleViews.Add(new ArticleView() { ArticleId = 110314, View = 18811 });
            _articleViews.Add(new ArticleView() { ArticleId = 110323, View = 4830 });
            _articleViews.Add(new ArticleView() { ArticleId = 110325, View = 824 });
            _articleViews.Add(new ArticleView() { ArticleId = 110455, View = 8161 });
            _articleViews.Add(new ArticleView() { ArticleId = 110458, View = 1483 });
            _articleViews.Add(new ArticleView() { ArticleId = 110507, View = 135 });
            _articleViews.Add(new ArticleView() { ArticleId = 110508, View = 620 });
            _articleViews.Add(new ArticleView() { ArticleId = 110509, View = 6037 });
            _articleViews.Add(new ArticleView() { ArticleId = 110511, View = 335 });
            _articleViews.Add(new ArticleView() { ArticleId = 110512, View = 185 });
            _articleViews.Add(new ArticleView() { ArticleId = 110415, View = 674 });
            _articleViews.Add(new ArticleView() { ArticleId = 110416, View = 1212 });
            _articleViews.Add(new ArticleView() { ArticleId = 110282, View = 538 });
            _articleViews.Add(new ArticleView() { ArticleId = 110553, View = 1004 });
            _articleViews.Add(new ArticleView() { ArticleId = 110554, View = 754 });
            _articleViews.Add(new ArticleView() { ArticleId = 110555, View = 413 });
            _articleViews.Add(new ArticleView() { ArticleId = 110466, View = 159 });
            _articleViews.Add(new ArticleView() { ArticleId = 110467, View = 5944 });
            _articleViews.Add(new ArticleView() { ArticleId = 110381, View = 1404 });
            _articleViews.Add(new ArticleView() { ArticleId = 110383, View = 3334 });
            _articleViews.Add(new ArticleView() { ArticleId = 110334, View = 938 });
            _articleViews.Add(new ArticleView() { ArticleId = 110335, View = 518 });
            _articleViews.Add(new ArticleView() { ArticleId = 110157, View = 789 });
            _articleViews.Add(new ArticleView() { ArticleId = 110159, View = 2047 });
            _articleViews.Add(new ArticleView() { ArticleId = 110513, View = 489 });
            _articleViews.Add(new ArticleView() { ArticleId = 110514, View = 1073 });
            _articleViews.Add(new ArticleView() { ArticleId = 110419, View = 246 });
            _articleViews.Add(new ArticleView() { ArticleId = 110420, View = 449 });
            _articleViews.Add(new ArticleView() { ArticleId = 110469, View = 1206 });
            _articleViews.Add(new ArticleView() { ArticleId = 110471, View = 825 });
            _articleViews.Add(new ArticleView() { ArticleId = 110421, View = 3161 });
            _articleViews.Add(new ArticleView() { ArticleId = 110422, View = 480 });
            _articleViews.Add(new ArticleView() { ArticleId = 110423, View = 5137 });
            _articleViews.Add(new ArticleView() { ArticleId = 110336, View = 685 });
            _articleViews.Add(new ArticleView() { ArticleId = 110339, View = 404 });
            _articleViews.Add(new ArticleView() { ArticleId = 110342, View = 1902 });
            _articleViews.Add(new ArticleView() { ArticleId = 110030, View = 786 });
            _articleViews.Add(new ArticleView() { ArticleId = 110044, View = 6606 });
            _articleViews.Add(new ArticleView() { ArticleId = 110516, View = 203 });
            _articleViews.Add(new ArticleView() { ArticleId = 110519, View = 79 });
            _articleViews.Add(new ArticleView() { ArticleId = 110522, View = 915 });
            _articleViews.Add(new ArticleView() { ArticleId = 110530, View = 968 });
            _articleViews.Add(new ArticleView() { ArticleId = 110344, View = 358 });
            _articleViews.Add(new ArticleView() { ArticleId = 110673, View = 278 });
            _articleViews.Add(new ArticleView() { ArticleId = 110675, View = 359 });
            _articleViews.Add(new ArticleView() { ArticleId = 110678, View = 250 });
            _articleViews.Add(new ArticleView() { ArticleId = 110681, View = 427 });
            _articleViews.Add(new ArticleView() { ArticleId = 110535, View = 932 });
            _articleViews.Add(new ArticleView() { ArticleId = 110539, View = 2092 });
            _articleViews.Add(new ArticleView() { ArticleId = 110543, View = 4848 });
            _articleViews.Add(new ArticleView() { ArticleId = 110616, View = 2715 });
            _articleViews.Add(new ArticleView() { ArticleId = 110619, View = 6687 });
            _articleViews.Add(new ArticleView() { ArticleId = 110557, View = 4940 });
            _articleViews.Add(new ArticleView() { ArticleId = 110558, View = 6206 });
            _articleViews.Add(new ArticleView() { ArticleId = 110682, View = 535 });
            _articleViews.Add(new ArticleView() { ArticleId = 110684, View = 716 });
            _articleViews.Add(new ArticleView() { ArticleId = 110692, View = 542 });
            _articleViews.Add(new ArticleView() { ArticleId = 110384, View = 2412 });
            _articleViews.Add(new ArticleView() { ArticleId = 110386, View = 8884 });
            _articleViews.Add(new ArticleView() { ArticleId = 110472, View = 826 });
            _articleViews.Add(new ArticleView() { ArticleId = 110473, View = 1197 });
            _articleViews.Add(new ArticleView() { ArticleId = 110559, View = 2485 });
            _articleViews.Add(new ArticleView() { ArticleId = 110567, View = 763 });
            _articleViews.Add(new ArticleView() { ArticleId = 110474, View = 548 });
            _articleViews.Add(new ArticleView() { ArticleId = 110475, View = 111 });
            _articleViews.Add(new ArticleView() { ArticleId = 110568, View = 382 });
            _articleViews.Add(new ArticleView() { ArticleId = 110571, View = 350 });
            _articleViews.Add(new ArticleView() { ArticleId = 110477, View = 1476 });
            _articleViews.Add(new ArticleView() { ArticleId = 110485, View = 228 });
            _articleViews.Add(new ArticleView() { ArticleId = 110486, View = 1021 });
            _articleViews.Add(new ArticleView() { ArticleId = 110572, View = 234 });
            _articleViews.Add(new ArticleView() { ArticleId = 110573, View = 477 });
            _articleViews.Add(new ArticleView() { ArticleId = 110574, View = 493 });
            _articleViews.Add(new ArticleView() { ArticleId = 110488, View = 1222 });
            _articleViews.Add(new ArticleView() { ArticleId = 110493, View = 530 });
            _articleViews.Add(new ArticleView() { ArticleId = 110575, View = 4811 });
            _articleViews.Add(new ArticleView() { ArticleId = 110576, View = 2139 });
            _articleViews.Add(new ArticleView() { ArticleId = 110577, View = 335 });
            _articleViews.Add(new ArticleView() { ArticleId = 110579, View = 730 });
            _articleViews.Add(new ArticleView() { ArticleId = 110583, View = 445 });
            _articleViews.Add(new ArticleView() { ArticleId = 110056, View = 5802 });
            _articleViews.Add(new ArticleView() { ArticleId = 110057, View = 549 });
            _articleViews.Add(new ArticleView() { ArticleId = 110774, View = 7350 });
            _articleViews.Add(new ArticleView() { ArticleId = 110776, View = 2482 });
            _articleViews.Add(new ArticleView() { ArticleId = 110781, View = 616 });
            _articleViews.Add(new ArticleView() { ArticleId = 110786, View = 341 });
            _articleViews.Add(new ArticleView() { ArticleId = 110794, View = 5017 });
            _articleViews.Add(new ArticleView() { ArticleId = 110796, View = 970 });
            _articleViews.Add(new ArticleView() { ArticleId = 110797, View = 577 });
            _articleViews.Add(new ArticleView() { ArticleId = 110798, View = 1019 });
            _articleViews.Add(new ArticleView() { ArticleId = 110799, View = 1766 });
            _articleViews.Add(new ArticleView() { ArticleId = 110800, View = 4010 });
            _articleViews.Add(new ArticleView() { ArticleId = 110803, View = 3786 });
            _articleViews.Add(new ArticleView() { ArticleId = 110804, View = 4876 });
            _articleViews.Add(new ArticleView() { ArticleId = 110725, View = 217 });
            _articleViews.Add(new ArticleView() { ArticleId = 110726, View = 189 });
            _articleViews.Add(new ArticleView() { ArticleId = 110727, View = 569 });
            _articleViews.Add(new ArticleView() { ArticleId = 110732, View = 524 });
            _articleViews.Add(new ArticleView() { ArticleId = 110733, View = 12966 });
            _articleViews.Add(new ArticleView() { ArticleId = 110735, View = 246 });
            _articleViews.Add(new ArticleView() { ArticleId = 110737, View = 769 });
            _articleViews.Add(new ArticleView() { ArticleId = 110738, View = 97 });
            _articleViews.Add(new ArticleView() { ArticleId = 110621, View = 843 });
            _articleViews.Add(new ArticleView() { ArticleId = 110626, View = 601 });
            _articleViews.Add(new ArticleView() { ArticleId = 110495, View = 16344 });
            _articleViews.Add(new ArticleView() { ArticleId = 110822, View = 2312 });
            _articleViews.Add(new ArticleView() { ArticleId = 110739, View = 2061 });
            _articleViews.Add(new ArticleView() { ArticleId = 110740, View = 5434 });
            _articleViews.Add(new ArticleView() { ArticleId = 110741, View = 676 });
            _articleViews.Add(new ArticleView() { ArticleId = 110549, View = 5844 });
            _articleViews.Add(new ArticleView() { ArticleId = 110551, View = 801 });
            _articleViews.Add(new ArticleView() { ArticleId = 110874, View = 1109 });
            _articleViews.Add(new ArticleView() { ArticleId = 110877, View = 3810 });
            _articleViews.Add(new ArticleView() { ArticleId = 110878, View = 4965 });
            _articleViews.Add(new ArticleView() { ArticleId = 110879, View = 12935 });
            _articleViews.Add(new ArticleView() { ArticleId = 110880, View = 5796 });
            _articleViews.Add(new ArticleView() { ArticleId = 110881, View = 1811 });
            _articleViews.Add(new ArticleView() { ArticleId = 110882, View = 1690 });
            _articleViews.Add(new ArticleView() { ArticleId = 110883, View = 169 });
            _articleViews.Add(new ArticleView() { ArticleId = 110884, View = 419 });
            _articleViews.Add(new ArticleView() { ArticleId = 110886, View = 185 });
            _articleViews.Add(new ArticleView() { ArticleId = 110889, View = 357 });
            _articleViews.Add(new ArticleView() { ArticleId = 110895, View = 1747 });
            _articleViews.Add(new ArticleView() { ArticleId = 110896, View = 7535 });
            _articleViews.Add(new ArticleView() { ArticleId = 110900, View = 1300 });
            _articleViews.Add(new ArticleView() { ArticleId = 110742, View = 1239 });
            _articleViews.Add(new ArticleView() { ArticleId = 110743, View = 198 });
            _articleViews.Add(new ArticleView() { ArticleId = 110745, View = 428 });
            _articleViews.Add(new ArticleView() { ArticleId = 110746, View = 1097 });
            _articleViews.Add(new ArticleView() { ArticleId = 110747, View = 585 });
            _articleViews.Add(new ArticleView() { ArticleId = 110748, View = 1826 });
            _articleViews.Add(new ArticleView() { ArticleId = 110751, View = 983 });
            _articleViews.Add(new ArticleView() { ArticleId = 110752, View = 916 });
            _articleViews.Add(new ArticleView() { ArticleId = 110755, View = 2099 });
            _articleViews.Add(new ArticleView() { ArticleId = 110762, View = 385 });
            _articleViews.Add(new ArticleView() { ArticleId = 110763, View = 11014 });
            _articleViews.Add(new ArticleView() { ArticleId = 110765, View = 1349 });
            _articleViews.Add(new ArticleView() { ArticleId = 110921, View = 340 });
            _articleViews.Add(new ArticleView() { ArticleId = 110924, View = 1004 });
            _articleViews.Add(new ArticleView() { ArticleId = 110928, View = 275 });
            _articleViews.Add(new ArticleView() { ArticleId = 110930, View = 487 });
            _articleViews.Add(new ArticleView() { ArticleId = 110931, View = 2323 });
            _articleViews.Add(new ArticleView() { ArticleId = 110938, View = 1796 });
            _articleViews.Add(new ArticleView() { ArticleId = 110940, View = 403 });
            _articleViews.Add(new ArticleView() { ArticleId = 110943, View = 1447 });
            _articleViews.Add(new ArticleView() { ArticleId = 110948, View = 752 });
            _articleViews.Add(new ArticleView() { ArticleId = 110950, View = 5620 });
            _articleViews.Add(new ArticleView() { ArticleId = 110627, View = 587 });
            _articleViews.Add(new ArticleView() { ArticleId = 110628, View = 6524 });
            _articleViews.Add(new ArticleView() { ArticleId = 110952, View = 789 });
            _articleViews.Add(new ArticleView() { ArticleId = 110959, View = 3059 });
            _articleViews.Add(new ArticleView() { ArticleId = 110960, View = 986 });
            _articleViews.Add(new ArticleView() { ArticleId = 110629, View = 547 });
            _articleViews.Add(new ArticleView() { ArticleId = 110634, View = 317 });
            _articleViews.Add(new ArticleView() { ArticleId = 110637, View = 134 });
            _articleViews.Add(new ArticleView() { ArticleId = 110909, View = 1419 });
            _articleViews.Add(new ArticleView() { ArticleId = 110918, View = 7688 });
            _articleViews.Add(new ArticleView() { ArticleId = 110641, View = 1773 });
            _articleViews.Add(new ArticleView() { ArticleId = 110642, View = 214 });
            _articleViews.Add(new ArticleView() { ArticleId = 110588, View = 1193 });
            _articleViews.Add(new ArticleView() { ArticleId = 110589, View = 6328 });
            _articleViews.Add(new ArticleView() { ArticleId = 110591, View = 457 });
            _articleViews.Add(new ArticleView() { ArticleId = 110643, View = 259 });
            _articleViews.Add(new ArticleView() { ArticleId = 110644, View = 187 });
            _articleViews.Add(new ArticleView() { ArticleId = 110647, View = 558 });
            _articleViews.Add(new ArticleView() { ArticleId = 110824, View = 1255 });
            _articleViews.Add(new ArticleView() { ArticleId = 110825, View = 2063 });
            _articleViews.Add(new ArticleView() { ArticleId = 110648, View = 580 });
            _articleViews.Add(new ArticleView() { ArticleId = 110654, View = 13363 });
            _articleViews.Add(new ArticleView() { ArticleId = 110832, View = 829 });
            _articleViews.Add(new ArticleView() { ArticleId = 110834, View = 16620 });
            _articleViews.Add(new ArticleView() { ArticleId = 110657, View = 121 });
            _articleViews.Add(new ArticleView() { ArticleId = 110658, View = 688 });
            _articleViews.Add(new ArticleView() { ArticleId = 110661, View = 2971 });
            _articleViews.Add(new ArticleView() { ArticleId = 110835, View = 12144 });
            _articleViews.Add(new ArticleView() { ArticleId = 110976, View = 162 });
            _articleViews.Add(new ArticleView() { ArticleId = 110977, View = 2587 });
            _articleViews.Add(new ArticleView() { ArticleId = 110978, View = 93 });
            _articleViews.Add(new ArticleView() { ArticleId = 110593, View = 975 });
            _articleViews.Add(new ArticleView() { ArticleId = 110595, View = 1913 });
            _articleViews.Add(new ArticleView() { ArticleId = 110596, View = 171 });
            _articleViews.Add(new ArticleView() { ArticleId = 110961, View = 1794 });
            _articleViews.Add(new ArticleView() { ArticleId = 110966, View = 412 });
            _articleViews.Add(new ArticleView() { ArticleId = 110967, View = 4952 });
            _articleViews.Add(new ArticleView() { ArticleId = 110968, View = 2404 });
            _articleViews.Add(new ArticleView() { ArticleId = 110662, View = 1297 });
            _articleViews.Add(new ArticleView() { ArticleId = 110663, View = 74 });
            _articleViews.Add(new ArticleView() { ArticleId = 111031, View = 1025 });
            _articleViews.Add(new ArticleView() { ArticleId = 111033, View = 848 });
            _articleViews.Add(new ArticleView() { ArticleId = 111036, View = 1608 });
            _articleViews.Add(new ArticleView() { ArticleId = 111040, View = 4900 });
            _articleViews.Add(new ArticleView() { ArticleId = 110695, View = 1175 });
            _articleViews.Add(new ArticleView() { ArticleId = 110697, View = 510 });
            _articleViews.Add(new ArticleView() { ArticleId = 110701, View = 1463 });
            _articleViews.Add(new ArticleView() { ArticleId = 111043, View = 593 });
            _articleViews.Add(new ArticleView() { ArticleId = 111044, View = 3657 });
            _articleViews.Add(new ArticleView() { ArticleId = 111045, View = 478 });
            _articleViews.Add(new ArticleView() { ArticleId = 110982, View = 2558 });
            _articleViews.Add(new ArticleView() { ArticleId = 110984, View = 3049 });
            _articleViews.Add(new ArticleView() { ArticleId = 110985, View = 1155 });
            _articleViews.Add(new ArticleView() { ArticleId = 110705, View = 11327 });
            _articleViews.Add(new ArticleView() { ArticleId = 110706, View = 6403 });
            _articleViews.Add(new ArticleView() { ArticleId = 110986, View = 13249 });
            _articleViews.Add(new ArticleView() { ArticleId = 110989, View = 294 });
            _articleViews.Add(new ArticleView() { ArticleId = 110836, View = 405 });
            _articleViews.Add(new ArticleView() { ArticleId = 110837, View = 20911 });
            _articleViews.Add(new ArticleView() { ArticleId = 110425, View = 976 });
            _articleViews.Add(new ArticleView() { ArticleId = 110426, View = 4056 });
            _articleViews.Add(new ArticleView() { ArticleId = 110990, View = 287 });
            _articleViews.Add(new ArticleView() { ArticleId = 110991, View = 11174 });
            _articleViews.Add(new ArticleView() { ArticleId = 110839, View = 478 });
            _articleViews.Add(new ArticleView() { ArticleId = 110841, View = 6496 });
            _articleViews.Add(new ArticleView() { ArticleId = 110666, View = 333 });
            _articleViews.Add(new ArticleView() { ArticleId = 111102, View = 1181 });
            _articleViews.Add(new ArticleView() { ArticleId = 111046, View = 4791 });
            _articleViews.Add(new ArticleView() { ArticleId = 111047, View = 403 });
            _articleViews.Add(new ArticleView() { ArticleId = 111050, View = 104 });
            _articleViews.Add(new ArticleView() { ArticleId = 111051, View = 189 });
            _articleViews.Add(new ArticleView() { ArticleId = 111052, View = 3709 });
            _articleViews.Add(new ArticleView() { ArticleId = 110710, View = 822 });
            _articleViews.Add(new ArticleView() { ArticleId = 110711, View = 1611 });
            _articleViews.Add(new ArticleView() { ArticleId = 110712, View = 580 });
            _articleViews.Add(new ArticleView() { ArticleId = 110713, View = 859 });
            _articleViews.Add(new ArticleView() { ArticleId = 111054, View = 357 });
            _articleViews.Add(new ArticleView() { ArticleId = 111056, View = 429 });
            _articleViews.Add(new ArticleView() { ArticleId = 111067, View = 703 });
            _articleViews.Add(new ArticleView() { ArticleId = 110992, View = 1053 });
            _articleViews.Add(new ArticleView() { ArticleId = 110993, View = 3765 });
            _articleViews.Add(new ArticleView() { ArticleId = 110994, View = 1327 });
            _articleViews.Add(new ArticleView() { ArticleId = 111068, View = 4860 });
            _articleViews.Add(new ArticleView() { ArticleId = 111069, View = 547 });
            _articleViews.Add(new ArticleView() { ArticleId = 111070, View = 488 });
            _articleViews.Add(new ArticleView() { ArticleId = 110996, View = 260 });
            _articleViews.Add(new ArticleView() { ArticleId = 110997, View = 359 });
            _articleViews.Add(new ArticleView() { ArticleId = 110999, View = 3277 });
            _articleViews.Add(new ArticleView() { ArticleId = 111071, View = 801 });
            _articleViews.Add(new ArticleView() { ArticleId = 111072, View = 573 });
            _articleViews.Add(new ArticleView() { ArticleId = 111073, View = 10751 });
            _articleViews.Add(new ArticleView() { ArticleId = 110714, View = 1828 });
            _articleViews.Add(new ArticleView() { ArticleId = 110716, View = 345 });
            _articleViews.Add(new ArticleView() { ArticleId = 111077, View = 7624 });
            _articleViews.Add(new ArticleView() { ArticleId = 111079, View = 285 });
            _articleViews.Add(new ArticleView() { ArticleId = 111084, View = 6494 });
            _articleViews.Add(new ArticleView() { ArticleId = 111093, View = 256 });
            _articleViews.Add(new ArticleView() { ArticleId = 110599, View = 151 });
            _articleViews.Add(new ArticleView() { ArticleId = 110600, View = 1477 });
            _articleViews.Add(new ArticleView() { ArticleId = 110607, View = 3731 });
            _articleViews.Add(new ArticleView() { ArticleId = 111000, View = 308 });
            _articleViews.Add(new ArticleView() { ArticleId = 111001, View = 5668 });
            _articleViews.Add(new ArticleView() { ArticleId = 111002, View = 3137 });
            _articleViews.Add(new ArticleView() { ArticleId = 110847, View = 4365 });
            _articleViews.Add(new ArticleView() { ArticleId = 110608, View = 807 });
            _articleViews.Add(new ArticleView() { ArticleId = 110611, View = 366 });
            _articleViews.Add(new ArticleView() { ArticleId = 110718, View = 1451 });
            _articleViews.Add(new ArticleView() { ArticleId = 110721, View = 464 });
            _articleViews.Add(new ArticleView() { ArticleId = 111006, View = 12492 });
            _articleViews.Add(new ArticleView() { ArticleId = 111015, View = 582 });
            _articleViews.Add(new ArticleView() { ArticleId = 111017, View = 3253 });
            _articleViews.Add(new ArticleView() { ArticleId = 110855, View = 2098 });
            _articleViews.Add(new ArticleView() { ArticleId = 110861, View = 2681 });
            _articleViews.Add(new ArticleView() { ArticleId = 111157, View = 1089 });
            _articleViews.Add(new ArticleView() { ArticleId = 111159, View = 381 });
            _articleViews.Add(new ArticleView() { ArticleId = 111198, View = 5172 });
            _articleViews.Add(new ArticleView() { ArticleId = 111202, View = 675 });
            _articleViews.Add(new ArticleView() { ArticleId = 111211, View = 3077 });
            _articleViews.Add(new ArticleView() { ArticleId = 111212, View = 637 });
            _articleViews.Add(new ArticleView() { ArticleId = 111213, View = 1044 });
            _articleViews.Add(new ArticleView() { ArticleId = 111160, View = 252 });
            _articleViews.Add(new ArticleView() { ArticleId = 111164, View = 32981 });
            _articleViews.Add(new ArticleView() { ArticleId = 111165, View = 1203 });
            _articleViews.Add(new ArticleView() { ArticleId = 111167, View = 488 });
            _articleViews.Add(new ArticleView() { ArticleId = 111168, View = 245 });
            _articleViews.Add(new ArticleView() { ArticleId = 111019, View = 883 });
            _articleViews.Add(new ArticleView() { ArticleId = 111020, View = 1067 });
            _articleViews.Add(new ArticleView() { ArticleId = 111023, View = 2029 });
            _articleViews.Add(new ArticleView() { ArticleId = 111026, View = 2089 });
            _articleViews.Add(new ArticleView() { ArticleId = 111103, View = 3686 });
            _articleViews.Add(new ArticleView() { ArticleId = 111105, View = 316 });
            _articleViews.Add(new ArticleView() { ArticleId = 111108, View = 2842 });
            _articleViews.Add(new ArticleView() { ArticleId = 110866, View = 8153 });
            _articleViews.Add(new ArticleView() { ArticleId = 110869, View = 2145 });
            _articleViews.Add(new ArticleView() { ArticleId = 111216, View = 775 });
            _articleViews.Add(new ArticleView() { ArticleId = 111217, View = 1266 });
            _articleViews.Add(new ArticleView() { ArticleId = 111259, View = 1069 });
            _articleViews.Add(new ArticleView() { ArticleId = 111263, View = 20 });
            _articleViews.Add(new ArticleView() { ArticleId = 110428, View = 1709 });
            _articleViews.Add(new ArticleView() { ArticleId = 110433, View = 476 });
            _articleViews.Add(new ArticleView() { ArticleId = 110439, View = 318 });
            _articleViews.Add(new ArticleView() { ArticleId = 111170, View = 177 });
            _articleViews.Add(new ArticleView() { ArticleId = 111172, View = 703 });
            _articleViews.Add(new ArticleView() { ArticleId = 111219, View = 1863 });
            _articleViews.Add(new ArticleView() { ArticleId = 111232, View = 474 });
            _articleViews.Add(new ArticleView() { ArticleId = 110871, View = 933 });
            _articleViews.Add(new ArticleView() { ArticleId = 110872, View = 44350 });
            _articleViews.Add(new ArticleView() { ArticleId = 111349, View = 919 });
            _articleViews.Add(new ArticleView() { ArticleId = 111264, View = 821 });
            _articleViews.Add(new ArticleView() { ArticleId = 111268, View = 767 });
            _articleViews.Add(new ArticleView() { ArticleId = 111233, View = 1119 });
            _articleViews.Add(new ArticleView() { ArticleId = 111234, View = 1376 });
            _articleViews.Add(new ArticleView() { ArticleId = 111242, View = 3 });
            _articleViews.Add(new ArticleView() { ArticleId = 111249, View = 1675 });
            _articleViews.Add(new ArticleView() { ArticleId = 111250, View = 6722 });
            _articleViews.Add(new ArticleView() { ArticleId = 111110, View = 639 });
            _articleViews.Add(new ArticleView() { ArticleId = 111112, View = 4719 });
            _articleViews.Add(new ArticleView() { ArticleId = 111115, View = 2087 });
            _articleViews.Add(new ArticleView() { ArticleId = 111116, View = 256 });
            _articleViews.Add(new ArticleView() { ArticleId = 111120, View = 960 });
            _articleViews.Add(new ArticleView() { ArticleId = 111121, View = 4865 });
            _articleViews.Add(new ArticleView() { ArticleId = 111354, View = 797 });
            _articleViews.Add(new ArticleView() { ArticleId = 111356, View = 609 });
            _articleViews.Add(new ArticleView() { ArticleId = 111364, View = 172 });
            _articleViews.Add(new ArticleView() { ArticleId = 111174, View = 930 });
            _articleViews.Add(new ArticleView() { ArticleId = 111175, View = 26505 });
            _articleViews.Add(new ArticleView() { ArticleId = 111251, View = 6598 });
            _articleViews.Add(new ArticleView() { ArticleId = 111254, View = 1912 });
            _articleViews.Add(new ArticleView() { ArticleId = 111425, View = 1242 });
            _articleViews.Add(new ArticleView() { ArticleId = 111429, View = 1242 });
            _articleViews.Add(new ArticleView() { ArticleId = 111365, View = 3561 });
            _articleViews.Add(new ArticleView() { ArticleId = 111369, View = 673 });
            _articleViews.Add(new ArticleView() { ArticleId = 111284, View = 868 });
            _articleViews.Add(new ArticleView() { ArticleId = 111288, View = 6810 });
            _articleViews.Add(new ArticleView() { ArticleId = 111430, View = 291 });
            _articleViews.Add(new ArticleView() { ArticleId = 111431, View = 222 });
            _articleViews.Add(new ArticleView() { ArticleId = 111432, View = 646 });
            _articleViews.Add(new ArticleView() { ArticleId = 111373, View = 177 });
            _articleViews.Add(new ArticleView() { ArticleId = 111377, View = 383 });
            _articleViews.Add(new ArticleView() { ArticleId = 111381, View = 3974 });
            _articleViews.Add(new ArticleView() { ArticleId = 111382, View = 1780 });
            _articleViews.Add(new ArticleView() { ArticleId = 111383, View = 1235 });
            _articleViews.Add(new ArticleView() { ArticleId = 111384, View = 2365 });
            _articleViews.Add(new ArticleView() { ArticleId = 111176, View = 6173 });
            _articleViews.Add(new ArticleView() { ArticleId = 111180, View = 9011 });
            _articleViews.Add(new ArticleView() { ArticleId = 111306, View = 1937 });
            _articleViews.Add(new ArticleView() { ArticleId = 111307, View = 9105 });
            _articleViews.Add(new ArticleView() { ArticleId = 111309, View = 2764 });
            _articleViews.Add(new ArticleView() { ArticleId = 111311, View = 4626 });
            _articleViews.Add(new ArticleView() { ArticleId = 111181, View = 2926 });
            _articleViews.Add(new ArticleView() { ArticleId = 111182, View = 2392 });
            _articleViews.Add(new ArticleView() { ArticleId = 111438, View = 657 });
            _articleViews.Add(new ArticleView() { ArticleId = 111442, View = 936 });
            _articleViews.Add(new ArticleView() { ArticleId = 111444, View = 275 });
            _articleViews.Add(new ArticleView() { ArticleId = 111122, View = 834 });
            _articleViews.Add(new ArticleView() { ArticleId = 111123, View = 5595 });
            _articleViews.Add(new ArticleView() { ArticleId = 111299, View = 22386 });
            _articleViews.Add(new ArticleView() { ArticleId = 111300, View = 633 });
            _articleViews.Add(new ArticleView() { ArticleId = 111301, View = 3187 });
            _articleViews.Add(new ArticleView() { ArticleId = 111302, View = 543 });
            _articleViews.Add(new ArticleView() { ArticleId = 111303, View = 728 });
            _articleViews.Add(new ArticleView() { ArticleId = 111385, View = 2348 });
            _articleViews.Add(new ArticleView() { ArticleId = 111386, View = 2582 });
            _articleViews.Add(new ArticleView() { ArticleId = 111315, View = 2243 });
            _articleViews.Add(new ArticleView() { ArticleId = 111316, View = 1736 });
            _articleViews.Add(new ArticleView() { ArticleId = 111317, View = 1315 });
            _articleViews.Add(new ArticleView() { ArticleId = 111318, View = 565 });
            _articleViews.Add(new ArticleView() { ArticleId = 111320, View = 3905 });
            _articleViews.Add(new ArticleView() { ArticleId = 111321, View = 353 });
            _articleViews.Add(new ArticleView() { ArticleId = 111322, View = 72 });
            _articleViews.Add(new ArticleView() { ArticleId = 111324, View = 199 });
            _articleViews.Add(new ArticleView() { ArticleId = 111327, View = 149 });
            _articleViews.Add(new ArticleView() { ArticleId = 111331, View = 2543 });
            _articleViews.Add(new ArticleView() { ArticleId = 111332, View = 8000 });
            _articleViews.Add(new ArticleView() { ArticleId = 111387, View = 981 });
            _articleViews.Add(new ArticleView() { ArticleId = 111388, View = 369 });
            _articleViews.Add(new ArticleView() { ArticleId = 111389, View = 989 });
            _articleViews.Add(new ArticleView() { ArticleId = 111304, View = 10854 });
            _articleViews.Add(new ArticleView() { ArticleId = 111305, View = 10985 });
            _articleViews.Add(new ArticleView() { ArticleId = 111390, View = 9988 });
            _articleViews.Add(new ArticleView() { ArticleId = 111406, View = 437 });
            _articleViews.Add(new ArticleView() { ArticleId = 111414, View = 710 });
            _articleViews.Add(new ArticleView() { ArticleId = 111507, View = 768 });
            _articleViews.Add(new ArticleView() { ArticleId = 111446, View = 5147 });
            _articleViews.Add(new ArticleView() { ArticleId = 111447, View = 220 });
            _articleViews.Add(new ArticleView() { ArticleId = 111335, View = 132 });
            _articleViews.Add(new ArticleView() { ArticleId = 111336, View = 482 });
            _articleViews.Add(new ArticleView() { ArticleId = 111338, View = 10164 });
            _articleViews.Add(new ArticleView() { ArticleId = 111186, View = 20811 });
            _articleViews.Add(new ArticleView() { ArticleId = 111197, View = 1875 });
            _articleViews.Add(new ArticleView() { ArticleId = 111464, View = 7588 });
            _articleViews.Add(new ArticleView() { ArticleId = 111472, View = 2123 });
            _articleViews.Add(new ArticleView() { ArticleId = 111567, View = 691 });
            _articleViews.Add(new ArticleView() { ArticleId = 111568, View = 855 });
            _articleViews.Add(new ArticleView() { ArticleId = 111125, View = 866 });
            _articleViews.Add(new ArticleView() { ArticleId = 111126, View = 1310 });
            _articleViews.Add(new ArticleView() { ArticleId = 111509, View = 1858 });
            _articleViews.Add(new ArticleView() { ArticleId = 111511, View = 114 });
            _articleViews.Add(new ArticleView() { ArticleId = 111448, View = 964 });
            _articleViews.Add(new ArticleView() { ArticleId = 111450, View = 983 });
            _articleViews.Add(new ArticleView() { ArticleId = 111342, View = 445 });
            _articleViews.Add(new ArticleView() { ArticleId = 111343, View = 112 });
            _articleViews.Add(new ArticleView() { ArticleId = 111344, View = 832 });
            _articleViews.Add(new ArticleView() { ArticleId = 111345, View = 7814 });
            _articleViews.Add(new ArticleView() { ArticleId = 111347, View = 969 });
            _articleViews.Add(new ArticleView() { ArticleId = 111569, View = 1924 });
            _articleViews.Add(new ArticleView() { ArticleId = 111570, View = 1190 });
            _articleViews.Add(new ArticleView() { ArticleId = 111127, View = 542 });
            _articleViews.Add(new ArticleView() { ArticleId = 111128, View = 556 });
            _articleViews.Add(new ArticleView() { ArticleId = 111512, View = 10016 });
            _articleViews.Add(new ArticleView() { ArticleId = 111514, View = 1565 });
            _articleViews.Add(new ArticleView() { ArticleId = 111516, View = 207 });
            _articleViews.Add(new ArticleView() { ArticleId = 111518, View = 436 });
            _articleViews.Add(new ArticleView() { ArticleId = 111522, View = 480 });
            _articleViews.Add(new ArticleView() { ArticleId = 111525, View = 29754 });
            _articleViews.Add(new ArticleView() { ArticleId = 111529, View = 1163 });
            _articleViews.Add(new ArticleView() { ArticleId = 111535, View = 9257 });
            _articleViews.Add(new ArticleView() { ArticleId = 111451, View = 273 });
            _articleViews.Add(new ArticleView() { ArticleId = 111456, View = 576 });
            _articleViews.Add(new ArticleView() { ArticleId = 111670, View = 791 });
            _articleViews.Add(new ArticleView() { ArticleId = 111671, View = 278 });
            _articleViews.Add(new ArticleView() { ArticleId = 111625, View = 631 });
            _articleViews.Add(new ArticleView() { ArticleId = 111628, View = 10136 });
            _articleViews.Add(new ArticleView() { ArticleId = 111580, View = 2595 });
            _articleViews.Add(new ArticleView() { ArticleId = 111583, View = 667 });
            _articleViews.Add(new ArticleView() { ArticleId = 111477, View = 494 });
            _articleViews.Add(new ArticleView() { ArticleId = 111481, View = 2805 });
            _articleViews.Add(new ArticleView() { ArticleId = 111482, View = 1745 });
            _articleViews.Add(new ArticleView() { ArticleId = 111536, View = 778 });
            _articleViews.Add(new ArticleView() { ArticleId = 111537, View = 1916 });
            _articleViews.Add(new ArticleView() { ArticleId = 111631, View = 271 });
            _articleViews.Add(new ArticleView() { ArticleId = 111634, View = 7709 });
            _articleViews.Add(new ArticleView() { ArticleId = 111635, View = 444 });
            _articleViews.Add(new ArticleView() { ArticleId = 111541, View = 392 });
            _articleViews.Add(new ArticleView() { ArticleId = 111551, View = 14012 });
            _articleViews.Add(new ArticleView() { ArticleId = 111555, View = 238 });
            _articleViews.Add(new ArticleView() { ArticleId = 111677, View = 1189 });
            _articleViews.Add(new ArticleView() { ArticleId = 111679, View = 3608 });
            _articleViews.Add(new ArticleView() { ArticleId = 111483, View = 302 });
            _articleViews.Add(new ArticleView() { ArticleId = 111492, View = 88 });
            _articleViews.Add(new ArticleView() { ArticleId = 111640, View = 445 });
            _articleViews.Add(new ArticleView() { ArticleId = 111641, View = 520 });
            _articleViews.Add(new ArticleView() { ArticleId = 111643, View = 302 });
            _articleViews.Add(new ArticleView() { ArticleId = 111493, View = 405 });
            _articleViews.Add(new ArticleView() { ArticleId = 111494, View = 90 });
            _articleViews.Add(new ArticleView() { ArticleId = 111584, View = 5807 });
            _articleViews.Add(new ArticleView() { ArticleId = 111586, View = 340 });
            _articleViews.Add(new ArticleView() { ArticleId = 111589, View = 236 });
            _articleViews.Add(new ArticleView() { ArticleId = 111647, View = 947 });
            _articleViews.Add(new ArticleView() { ArticleId = 111648, View = 3297 });
            _articleViews.Add(new ArticleView() { ArticleId = 111129, View = 603 });
            _articleViews.Add(new ArticleView() { ArticleId = 111142, View = 585 });
            _articleViews.Add(new ArticleView() { ArticleId = 111143, View = 3533 });
            _articleViews.Add(new ArticleView() { ArticleId = 111144, View = 500 });
            _articleViews.Add(new ArticleView() { ArticleId = 111683, View = 234 });
            _articleViews.Add(new ArticleView() { ArticleId = 111685, View = 782 });
            _articleViews.Add(new ArticleView() { ArticleId = 111686, View = 8531 });
            _articleViews.Add(new ArticleView() { ArticleId = 111562, View = 2932 });
            _articleViews.Add(new ArticleView() { ArticleId = 111563, View = 6867 });
            _articleViews.Add(new ArticleView() { ArticleId = 111590, View = 1340 });
            _articleViews.Add(new ArticleView() { ArticleId = 111591, View = 1417 });
            _articleViews.Add(new ArticleView() { ArticleId = 111649, View = 1151 });
            _articleViews.Add(new ArticleView() { ArticleId = 111650, View = 472 });
            _articleViews.Add(new ArticleView() { ArticleId = 111145, View = 6771 });
            _articleViews.Add(new ArticleView() { ArticleId = 111147, View = 9420 });
            _articleViews.Add(new ArticleView() { ArticleId = 111687, View = 27120 });
            _articleViews.Add(new ArticleView() { ArticleId = 111689, View = 589 });
            _articleViews.Add(new ArticleView() { ArticleId = 111565, View = 12967 });
            _articleViews.Add(new ArticleView() { ArticleId = 111566, View = 309 });
            _articleViews.Add(new ArticleView() { ArticleId = 111731, View = 1167 });
            _articleViews.Add(new ArticleView() { ArticleId = 111495, View = 122 });
            _articleViews.Add(new ArticleView() { ArticleId = 111498, View = 263 });
            _articleViews.Add(new ArticleView() { ArticleId = 111592, View = 5298 });
            _articleViews.Add(new ArticleView() { ArticleId = 111594, View = 510 });
            _articleViews.Add(new ArticleView() { ArticleId = 111651, View = 675 });
            _articleViews.Add(new ArticleView() { ArticleId = 111652, View = 43549 });
            _articleViews.Add(new ArticleView() { ArticleId = 111148, View = 3130 });
            _articleViews.Add(new ArticleView() { ArticleId = 111150, View = 1599 });
            _articleViews.Add(new ArticleView() { ArticleId = 111151, View = 554 });
            _articleViews.Add(new ArticleView() { ArticleId = 111692, View = 596 });
            _articleViews.Add(new ArticleView() { ArticleId = 111693, View = 282 });
            _articleViews.Add(new ArticleView() { ArticleId = 111747, View = 3716 });
            _articleViews.Add(new ArticleView() { ArticleId = 111748, View = 4701 });
            _articleViews.Add(new ArticleView() { ArticleId = 111751, View = 261 });
            _articleViews.Add(new ArticleView() { ArticleId = 111752, View = 177 });
            _articleViews.Add(new ArticleView() { ArticleId = 111753, View = 827 });
            _articleViews.Add(new ArticleView() { ArticleId = 111754, View = 622 });
            _articleViews.Add(new ArticleView() { ArticleId = 111595, View = 4821 });
            _articleViews.Add(new ArticleView() { ArticleId = 111596, View = 661 });
            _articleViews.Add(new ArticleView() { ArticleId = 111597, View = 376 });
            _articleViews.Add(new ArticleView() { ArticleId = 111653, View = 908 });
            _articleViews.Add(new ArticleView() { ArticleId = 111654, View = 1448 });
            _articleViews.Add(new ArticleView() { ArticleId = 111782, View = 7458 });
            _articleViews.Add(new ArticleView() { ArticleId = 111784, View = 309 });
            _articleViews.Add(new ArticleView() { ArticleId = 111786, View = 707 });
            _articleViews.Add(new ArticleView() { ArticleId = 111787, View = 3325 });
            _articleViews.Add(new ArticleView() { ArticleId = 111792, View = 1586 });
            _articleViews.Add(new ArticleView() { ArticleId = 111698, View = 204 });
            _articleViews.Add(new ArticleView() { ArticleId = 111709, View = 939 });
            _articleViews.Add(new ArticleView() { ArticleId = 111755, View = 1622 });
            _articleViews.Add(new ArticleView() { ArticleId = 111756, View = 31863 });
            _articleViews.Add(new ArticleView() { ArticleId = 111767, View = 1388 });
            _articleViews.Add(new ArticleView() { ArticleId = 111772, View = 453 });
            _articleViews.Add(new ArticleView() { ArticleId = 111775, View = 16398 });
            _articleViews.Add(new ArticleView() { ArticleId = 111720, View = 5133 });
            _articleViews.Add(new ArticleView() { ArticleId = 111721, View = 1234 });
            _articleViews.Add(new ArticleView() { ArticleId = 111500, View = 39745 });
            _articleViews.Add(new ArticleView() { ArticleId = 111502, View = 3575 });
            _articleViews.Add(new ArticleView() { ArticleId = 111503, View = 239 });
            _articleViews.Add(new ArticleView() { ArticleId = 111505, View = 473 });
            _articleViews.Add(new ArticleView() { ArticleId = 111722, View = 412 });
            _articleViews.Add(new ArticleView() { ArticleId = 111724, View = 204 });
            _articleViews.Add(new ArticleView() { ArticleId = 111655, View = 1269 });
            _articleViews.Add(new ArticleView() { ArticleId = 111656, View = 927 });
            _articleViews.Add(new ArticleView() { ArticleId = 111657, View = 629 });
            _articleViews.Add(new ArticleView() { ArticleId = 111835, View = 3788 });
            _articleViews.Add(new ArticleView() { ArticleId = 111838, View = 610 });
            _articleViews.Add(new ArticleView() { ArticleId = 111839, View = 361 });
            _articleViews.Add(new ArticleView() { ArticleId = 111776, View = 219 });
            _articleViews.Add(new ArticleView() { ArticleId = 111889, View = 3094 });
            _articleViews.Add(new ArticleView() { ArticleId = 111892, View = 318 });
            _articleViews.Add(new ArticleView() { ArticleId = 111894, View = 707 });
            _articleViews.Add(new ArticleView() { ArticleId = 111908, View = 122 });
            _articleViews.Add(new ArticleView() { ArticleId = 111910, View = 389 });
            _articleViews.Add(new ArticleView() { ArticleId = 111725, View = 283 });
            _articleViews.Add(new ArticleView() { ArticleId = 111726, View = 641 });
            _articleViews.Add(new ArticleView() { ArticleId = 111663, View = 148 });
            _articleViews.Add(new ArticleView() { ArticleId = 111667, View = 8797 });
            _articleViews.Add(new ArticleView() { ArticleId = 111668, View = 4865 });
            _articleViews.Add(new ArticleView() { ArticleId = 111840, View = 1452 });
            _articleViews.Add(new ArticleView() { ArticleId = 111841, View = 3067 });
            _articleViews.Add(new ArticleView() { ArticleId = 111669, View = 921 });
            _articleViews.Add(new ArticleView() { ArticleId = 111942, View = 549 });
            _articleViews.Add(new ArticleView() { ArticleId = 111944, View = 501 });
            _articleViews.Add(new ArticleView() { ArticleId = 111599, View = 4581 });
            _articleViews.Add(new ArticleView() { ArticleId = 111600, View = 14633 });
            _articleViews.Add(new ArticleView() { ArticleId = 111601, View = 338 });
            _articleViews.Add(new ArticleView() { ArticleId = 111842, View = 3506 });
            _articleViews.Add(new ArticleView() { ArticleId = 111852, View = 1689 });
            _articleViews.Add(new ArticleView() { ArticleId = 111853, View = 733 });
            _articleViews.Add(new ArticleView() { ArticleId = 111946, View = 241 });
            _articleViews.Add(new ArticleView() { ArticleId = 111949, View = 494 });
            _articleViews.Add(new ArticleView() { ArticleId = 111856, View = 2074 });
            _articleViews.Add(new ArticleView() { ArticleId = 111860, View = 475 });
            _articleViews.Add(new ArticleView() { ArticleId = 111950, View = 3422 });
            _articleViews.Add(new ArticleView() { ArticleId = 111955, View = 101 });
            _articleViews.Add(new ArticleView() { ArticleId = 111861, View = 1254 });
            _articleViews.Add(new ArticleView() { ArticleId = 111862, View = 4294 });
            _articleViews.Add(new ArticleView() { ArticleId = 111863, View = 10223 });
            _articleViews.Add(new ArticleView() { ArticleId = 111868, View = 12614 });
            _articleViews.Add(new ArticleView() { ArticleId = 111912, View = 125 });
            _articleViews.Add(new ArticleView() { ArticleId = 111918, View = 115 });
            _articleViews.Add(new ArticleView() { ArticleId = 111920, View = 272 });
            _articleViews.Add(new ArticleView() { ArticleId = 111729, View = 317 });
            _articleViews.Add(new ArticleView() { ArticleId = 111996, View = 672 });
            _articleViews.Add(new ArticleView() { ArticleId = 111876, View = 4356 });
            _articleViews.Add(new ArticleView() { ArticleId = 111879, View = 10316 });
            _articleViews.Add(new ArticleView() { ArticleId = 111922, View = 3633 });
            _articleViews.Add(new ArticleView() { ArticleId = 111923, View = 2092 });
            _articleViews.Add(new ArticleView() { ArticleId = 111924, View = 131 });
            _articleViews.Add(new ArticleView() { ArticleId = 111925, View = 352 });
            _articleViews.Add(new ArticleView() { ArticleId = 111926, View = 19682 });
            _articleViews.Add(new ArticleView() { ArticleId = 111927, View = 8533 });
            _articleViews.Add(new ArticleView() { ArticleId = 111933, View = 1380 });
            _articleViews.Add(new ArticleView() { ArticleId = 111934, View = 1200 });
            _articleViews.Add(new ArticleView() { ArticleId = 111935, View = 681 });
            _articleViews.Add(new ArticleView() { ArticleId = 111941, View = 351 });
            _articleViews.Add(new ArticleView() { ArticleId = 111793, View = 12952 });
            _articleViews.Add(new ArticleView() { ArticleId = 111797, View = 2068 });
            _articleViews.Add(new ArticleView() { ArticleId = 111606, View = 1141 });
            _articleViews.Add(new ArticleView() { ArticleId = 111607, View = 232 });
            _articleViews.Add(new ArticleView() { ArticleId = 112044, View = 3538 });
            _articleViews.Add(new ArticleView() { ArticleId = 112053, View = 431 });
            _articleViews.Add(new ArticleView() { ArticleId = 111098, View = 20026 });
            _articleViews.Add(new ArticleView() { ArticleId = 112094, View = 1552 });
            _articleViews.Add(new ArticleView() { ArticleId = 111881, View = 1880 });
            _articleViews.Add(new ArticleView() { ArticleId = 111884, View = 2309 });
            _articleViews.Add(new ArticleView() { ArticleId = 111957, View = 434 });
            _articleViews.Add(new ArticleView() { ArticleId = 111959, View = 505 });
            _articleViews.Add(new ArticleView() { ArticleId = 111962, View = 2659 });
            _articleViews.Add(new ArticleView() { ArticleId = 111997, View = 1389 });
            _articleViews.Add(new ArticleView() { ArticleId = 111998, View = 4382 });
            _articleViews.Add(new ArticleView() { ArticleId = 111886, View = 3324 });
            _articleViews.Add(new ArticleView() { ArticleId = 111887, View = 8899 });
            _articleViews.Add(new ArticleView() { ArticleId = 111888, View = 1804 });
            _articleViews.Add(new ArticleView() { ArticleId = 111798, View = 7214 });
            _articleViews.Add(new ArticleView() { ArticleId = 111799, View = 10164 });
            _articleViews.Add(new ArticleView() { ArticleId = 111800, View = 1363 });
            _articleViews.Add(new ArticleView() { ArticleId = 111801, View = 641 });
            _articleViews.Add(new ArticleView() { ArticleId = 111803, View = 6152 });
            _articleViews.Add(new ArticleView() { ArticleId = 111810, View = 3497 });
            _articleViews.Add(new ArticleView() { ArticleId = 111814, View = 1040 });
            _articleViews.Add(new ArticleView() { ArticleId = 112149, View = 7562 });
            _articleViews.Add(new ArticleView() { ArticleId = 112001, View = 1259 });
            _articleViews.Add(new ArticleView() { ArticleId = 112002, View = 671 });
            _articleViews.Add(new ArticleView() { ArticleId = 112003, View = 115 });
            _articleViews.Add(new ArticleView() { ArticleId = 112154, View = 3936 });
            _articleViews.Add(new ArticleView() { ArticleId = 111609, View = 1254 });
            _articleViews.Add(new ArticleView() { ArticleId = 111611, View = 5517 });
            _articleViews.Add(new ArticleView() { ArticleId = 111612, View = 830 });
            _articleViews.Add(new ArticleView() { ArticleId = 111615, View = 483 });
            _articleViews.Add(new ArticleView() { ArticleId = 111617, View = 765 });
            _articleViews.Add(new ArticleView() { ArticleId = 111965, View = 1782 });
            _articleViews.Add(new ArticleView() { ArticleId = 111968, View = 1528 });
            _articleViews.Add(new ArticleView() { ArticleId = 111969, View = 3226 });
            _articleViews.Add(new ArticleView() { ArticleId = 111970, View = 1602 });
            _articleViews.Add(new ArticleView() { ArticleId = 111971, View = 731 });
            _articleViews.Add(new ArticleView() { ArticleId = 111972, View = 1395 });
            _articleViews.Add(new ArticleView() { ArticleId = 111973, View = 652 });
            _articleViews.Add(new ArticleView() { ArticleId = 112157, View = 7306 });
            _articleViews.Add(new ArticleView() { ArticleId = 112159, View = 1080 });
            _articleViews.Add(new ArticleView() { ArticleId = 112160, View = 5872 });
            _articleViews.Add(new ArticleView() { ArticleId = 112162, View = 50780 });
            _articleViews.Add(new ArticleView() { ArticleId = 112164, View = 613 });
            _articleViews.Add(new ArticleView() { ArticleId = 112165, View = 448 });
            _articleViews.Add(new ArticleView() { ArticleId = 112169, View = 947 });
            _articleViews.Add(new ArticleView() { ArticleId = 112174, View = 827 });
            _articleViews.Add(new ArticleView() { ArticleId = 112057, View = 250 });
            _articleViews.Add(new ArticleView() { ArticleId = 112058, View = 579 });
            _articleViews.Add(new ArticleView() { ArticleId = 112059, View = 1132 });
            _articleViews.Add(new ArticleView() { ArticleId = 112062, View = 97 });
            _articleViews.Add(new ArticleView() { ArticleId = 112066, View = 628 });
            _articleViews.Add(new ArticleView() { ArticleId = 112004, View = 2427 });
            _articleViews.Add(new ArticleView() { ArticleId = 112010, View = 1212 });
            _articleViews.Add(new ArticleView() { ArticleId = 111619, View = 1204 });
            _articleViews.Add(new ArticleView() { ArticleId = 111620, View = 1364 });
            _articleViews.Add(new ArticleView() { ArticleId = 112183, View = 6183 });
            _articleViews.Add(new ArticleView() { ArticleId = 112189, View = 383 });
            _articleViews.Add(new ArticleView() { ArticleId = 111974, View = 730 });
            _articleViews.Add(new ArticleView() { ArticleId = 111975, View = 2666 });
            _articleViews.Add(new ArticleView() { ArticleId = 111976, View = 534 });
            _articleViews.Add(new ArticleView() { ArticleId = 111983, View = 7789 });
            _articleViews.Add(new ArticleView() { ArticleId = 111987, View = 1923 });
            _articleViews.Add(new ArticleView() { ArticleId = 111815, View = 4256 });
            _articleViews.Add(new ArticleView() { ArticleId = 111816, View = 4924 });
            _articleViews.Add(new ArticleView() { ArticleId = 112015, View = 355 });
            _articleViews.Add(new ArticleView() { ArticleId = 112017, View = 620 });
            _articleViews.Add(new ArticleView() { ArticleId = 112022, View = 680 });
            _articleViews.Add(new ArticleView() { ArticleId = 112208, View = 881 });
            _articleViews.Add(new ArticleView() { ArticleId = 112193, View = 1099 });
            _articleViews.Add(new ArticleView() { ArticleId = 112023, View = 2867 });
            _articleViews.Add(new ArticleView() { ArticleId = 112024, View = 20791 });
            _articleViews.Add(new ArticleView() { ArticleId = 112202, View = 3517 });
            _articleViews.Add(new ArticleView() { ArticleId = 112203, View = 482 });
            _articleViews.Add(new ArticleView() { ArticleId = 112031, View = 1541 });
            _articleViews.Add(new ArticleView() { ArticleId = 112037, View = 343 });
            _articleViews.Add(new ArticleView() { ArticleId = 112038, View = 1658 });
            _articleViews.Add(new ArticleView() { ArticleId = 112204, View = 12301 });
            _articleViews.Add(new ArticleView() { ArticleId = 112205, View = 1366 });
            _articleViews.Add(new ArticleView() { ArticleId = 112207, View = 987 });
            _articleViews.Add(new ArticleView() { ArticleId = 112039, View = 883 });
            _articleViews.Add(new ArticleView() { ArticleId = 112040, View = 407 });
            _articleViews.Add(new ArticleView() { ArticleId = 112278, View = 1744 });
            _articleViews.Add(new ArticleView() { ArticleId = 112286, View = 2024 });
            _articleViews.Add(new ArticleView() { ArticleId = 111989, View = 1171 });
            _articleViews.Add(new ArticleView() { ArticleId = 111991, View = 678 });
            _articleViews.Add(new ArticleView() { ArticleId = 111995, View = 166 });
            _articleViews.Add(new ArticleView() { ArticleId = 112295, View = 1269 });
            _articleViews.Add(new ArticleView() { ArticleId = 112306, View = 1552 });
            _articleViews.Add(new ArticleView() { ArticleId = 112041, View = 1355 });
            _articleViews.Add(new ArticleView() { ArticleId = 112042, View = 2570 });
            _articleViews.Add(new ArticleView() { ArticleId = 112095, View = 1332 });
            _articleViews.Add(new ArticleView() { ArticleId = 112096, View = 1882 });
            _articleViews.Add(new ArticleView() { ArticleId = 112097, View = 1239 });
            _articleViews.Add(new ArticleView() { ArticleId = 112043, View = 1518 });
            _articleViews.Add(new ArticleView() { ArticleId = 112405, View = 497 });
            _articleViews.Add(new ArticleView() { ArticleId = 112406, View = 2085 });
            _articleViews.Add(new ArticleView() { ArticleId = 112407, View = 606 });
            _articleViews.Add(new ArticleView() { ArticleId = 112408, View = 512 });
            _articleViews.Add(new ArticleView() { ArticleId = 112410, View = 1842 });
            _articleViews.Add(new ArticleView() { ArticleId = 112411, View = 5407 });
            _articleViews.Add(new ArticleView() { ArticleId = 112413, View = 4316 });
            _articleViews.Add(new ArticleView() { ArticleId = 112418, View = 2828 });
            _articleViews.Add(new ArticleView() { ArticleId = 112422, View = 1975 });
            _articleViews.Add(new ArticleView() { ArticleId = 112423, View = 724 });
            _articleViews.Add(new ArticleView() { ArticleId = 112068, View = 3670 });
            _articleViews.Add(new ArticleView() { ArticleId = 112069, View = 513 });
            _articleViews.Add(new ArticleView() { ArticleId = 112070, View = 598 });
            _articleViews.Add(new ArticleView() { ArticleId = 112349, View = 160 });
            _articleViews.Add(new ArticleView() { ArticleId = 112350, View = 2320 });
            _articleViews.Add(new ArticleView() { ArticleId = 112351, View = 2261 });
            _articleViews.Add(new ArticleView() { ArticleId = 112354, View = 4111 });
            _articleViews.Add(new ArticleView() { ArticleId = 112357, View = 12251 });
            _articleViews.Add(new ArticleView() { ArticleId = 112361, View = 4630 });
            _articleViews.Add(new ArticleView() { ArticleId = 112362, View = 623 });
            _articleViews.Add(new ArticleView() { ArticleId = 112366, View = 2030 });
            _articleViews.Add(new ArticleView() { ArticleId = 112369, View = 7664 });
            _articleViews.Add(new ArticleView() { ArticleId = 112372, View = 298 });
            _articleViews.Add(new ArticleView() { ArticleId = 112378, View = 14162 });
            _articleViews.Add(new ArticleView() { ArticleId = 112383, View = 359 });
            _articleViews.Add(new ArticleView() { ArticleId = 112071, View = 375 });
            _articleViews.Add(new ArticleView() { ArticleId = 112072, View = 115 });
            _articleViews.Add(new ArticleView() { ArticleId = 112074, View = 566 });
            _articleViews.Add(new ArticleView() { ArticleId = 112427, View = 1245 });
            _articleViews.Add(new ArticleView() { ArticleId = 112436, View = 996 });
            _articleViews.Add(new ArticleView() { ArticleId = 112076, View = 6475 });
            _articleViews.Add(new ArticleView() { ArticleId = 112077, View = 1389 });
            _articleViews.Add(new ArticleView() { ArticleId = 112387, View = 1299 });
            _articleViews.Add(new ArticleView() { ArticleId = 112390, View = 3529 });
            _articleViews.Add(new ArticleView() { ArticleId = 112391, View = 1647 });
            _articleViews.Add(new ArticleView() { ArticleId = 111817, View = 725 });
            _articleViews.Add(new ArticleView() { ArticleId = 111819, View = 3204 });
            _articleViews.Add(new ArticleView() { ArticleId = 112392, View = 187 });
            _articleViews.Add(new ArticleView() { ArticleId = 112393, View = 779 });
            _articleViews.Add(new ArticleView() { ArticleId = 112398, View = 3705 });
            _articleViews.Add(new ArticleView() { ArticleId = 112400, View = 899 });
            _articleViews.Add(new ArticleView() { ArticleId = 112462, View = 347 });
            _articleViews.Add(new ArticleView() { ArticleId = 112210, View = 1660 });
            _articleViews.Add(new ArticleView() { ArticleId = 112307, View = 1336 });
            _articleViews.Add(new ArticleView() { ArticleId = 112312, View = 391 });
            _articleViews.Add(new ArticleView() { ArticleId = 112313, View = 2031 });
            _articleViews.Add(new ArticleView() { ArticleId = 112463, View = 83 });
            _articleViews.Add(new ArticleView() { ArticleId = 112211, View = 2193 });
            _articleViews.Add(new ArticleView() { ArticleId = 112212, View = 1284 });
            _articleViews.Add(new ArticleView() { ArticleId = 111820, View = 4167 });
            _articleViews.Add(new ArticleView() { ArticleId = 111821, View = 419 });
            _articleViews.Add(new ArticleView() { ArticleId = 112443, View = 823 });
            _articleViews.Add(new ArticleView() { ArticleId = 112454, View = 304 });
            _articleViews.Add(new ArticleView() { ArticleId = 112078, View = 1030 });
            _articleViews.Add(new ArticleView() { ArticleId = 112079, View = 226 });
            _articleViews.Add(new ArticleView() { ArticleId = 112317, View = 904 });
            _articleViews.Add(new ArticleView() { ArticleId = 112318, View = 2030 });
            _articleViews.Add(new ArticleView() { ArticleId = 112321, View = 4547 });
            _articleViews.Add(new ArticleView() { ArticleId = 111822, View = 496 });
            _articleViews.Add(new ArticleView() { ArticleId = 111823, View = 1390 });
            _articleViews.Add(new ArticleView() { ArticleId = 111824, View = 476 });
            _articleViews.Add(new ArticleView() { ArticleId = 111826, View = 403 });
            _articleViews.Add(new ArticleView() { ArticleId = 112330, View = 593 });
            _articleViews.Add(new ArticleView() { ArticleId = 112331, View = 1874 });
            _articleViews.Add(new ArticleView() { ArticleId = 112464, View = 356 });
            _articleViews.Add(new ArticleView() { ArticleId = 112465, View = 2914 });
            _articleViews.Add(new ArticleView() { ArticleId = 112217, View = 18689 });
            _articleViews.Add(new ArticleView() { ArticleId = 112228, View = 406 });
            _articleViews.Add(new ArticleView() { ArticleId = 111827, View = 829 });
            _articleViews.Add(new ArticleView() { ArticleId = 111828, View = 1206 });
            _articleViews.Add(new ArticleView() { ArticleId = 112455, View = 14608 });
            _articleViews.Add(new ArticleView() { ArticleId = 112459, View = 1103 });
            _articleViews.Add(new ArticleView() { ArticleId = 112460, View = 7208 });
            _articleViews.Add(new ArticleView() { ArticleId = 111829, View = 1433 });
            _articleViews.Add(new ArticleView() { ArticleId = 111832, View = 593 });
            _articleViews.Add(new ArticleView() { ArticleId = 111833, View = 353 });
            _articleViews.Add(new ArticleView() { ArticleId = 112520, View = 6872 });
            _articleViews.Add(new ArticleView() { ArticleId = 112521, View = 583 });
            _articleViews.Add(new ArticleView() { ArticleId = 112522, View = 2578 });
            _articleViews.Add(new ArticleView() { ArticleId = 112466, View = 676 });
            _articleViews.Add(new ArticleView() { ArticleId = 112467, View = 665 });
            _articleViews.Add(new ArticleView() { ArticleId = 112468, View = 291 });
            _articleViews.Add(new ArticleView() { ArticleId = 112470, View = 9558 });
            _articleViews.Add(new ArticleView() { ArticleId = 112461, View = 232 });
            _articleViews.Add(new ArticleView() { ArticleId = 112581, View = 11072 });
            _articleViews.Add(new ArticleView() { ArticleId = 112081, View = 265 });
            _articleViews.Add(new ArticleView() { ArticleId = 112082, View = 1354 });
            _articleViews.Add(new ArticleView() { ArticleId = 112083, View = 3801 });
            _articleViews.Add(new ArticleView() { ArticleId = 112636, View = 15744 });
            _articleViews.Add(new ArticleView() { ArticleId = 112637, View = 3382 });
            _articleViews.Add(new ArticleView() { ArticleId = 112098, View = 469 });
            _articleViews.Add(new ArticleView() { ArticleId = 112099, View = 535 });
            _articleViews.Add(new ArticleView() { ArticleId = 112100, View = 173 });
            _articleViews.Add(new ArticleView() { ArticleId = 112101, View = 363 });
            _articleViews.Add(new ArticleView() { ArticleId = 112103, View = 280 });
            _articleViews.Add(new ArticleView() { ArticleId = 112523, View = 1943 });
            _articleViews.Add(new ArticleView() { ArticleId = 112524, View = 3307 });
            _articleViews.Add(new ArticleView() { ArticleId = 112472, View = 96 });
            _articleViews.Add(new ArticleView() { ArticleId = 112476, View = 119 });
            _articleViews.Add(new ArticleView() { ArticleId = 112477, View = 567 });
            _articleViews.Add(new ArticleView() { ArticleId = 112233, View = 607 });
            _articleViews.Add(new ArticleView() { ArticleId = 112234, View = 315 });
            _articleViews.Add(new ArticleView() { ArticleId = 112236, View = 672 });
            _articleViews.Add(new ArticleView() { ArticleId = 112339, View = 1940 });
            _articleViews.Add(new ArticleView() { ArticleId = 112341, View = 238 });
            _articleViews.Add(new ArticleView() { ArticleId = 112342, View = 2268 });
            _articleViews.Add(new ArticleView() { ArticleId = 112582, View = 1720 });
            _articleViews.Add(new ArticleView() { ArticleId = 112583, View = 19412 });
            _articleViews.Add(new ArticleView() { ArticleId = 112584, View = 10667 });
            _articleViews.Add(new ArticleView() { ArticleId = 112585, View = 656 });
            _articleViews.Add(new ArticleView() { ArticleId = 112480, View = 138 });
            _articleViews.Add(new ArticleView() { ArticleId = 112481, View = 722 });
            _articleViews.Add(new ArticleView() { ArticleId = 112482, View = 314 });
            _articleViews.Add(new ArticleView() { ArticleId = 112586, View = 347 });
            _articleViews.Add(new ArticleView() { ArticleId = 112587, View = 285 });
            _articleViews.Add(new ArticleView() { ArticleId = 112588, View = 22197 });
            _articleViews.Add(new ArticleView() { ArticleId = 112525, View = 289 });
            _articleViews.Add(new ArticleView() { ArticleId = 112526, View = 706 });
            _articleViews.Add(new ArticleView() { ArticleId = 112527, View = 8124 });
            _articleViews.Add(new ArticleView() { ArticleId = 112528, View = 680 });
            _articleViews.Add(new ArticleView() { ArticleId = 112638, View = 767 });
            _articleViews.Add(new ArticleView() { ArticleId = 112639, View = 2140 });
            _articleViews.Add(new ArticleView() { ArticleId = 112640, View = 1091 });
            _articleViews.Add(new ArticleView() { ArticleId = 112589, View = 569 });
            _articleViews.Add(new ArticleView() { ArticleId = 112590, View = 7603 });
            _articleViews.Add(new ArticleView() { ArticleId = 112238, View = 17227 });
            _articleViews.Add(new ArticleView() { ArticleId = 112239, View = 309 });
            _articleViews.Add(new ArticleView() { ArticleId = 112484, View = 224 });
            _articleViews.Add(new ArticleView() { ArticleId = 112488, View = 5599 });
            _articleViews.Add(new ArticleView() { ArticleId = 112492, View = 600 });
            _articleViews.Add(new ArticleView() { ArticleId = 112493, View = 451 });
            _articleViews.Add(new ArticleView() { ArticleId = 112495, View = 801 });
            _articleViews.Add(new ArticleView() { ArticleId = 112497, View = 9155 });
            _articleViews.Add(new ArticleView() { ArticleId = 112499, View = 2071 });
            _articleViews.Add(new ArticleView() { ArticleId = 112240, View = 2696 });
            _articleViews.Add(new ArticleView() { ArticleId = 112592, View = 1644 });
            _articleViews.Add(new ArticleView() { ArticleId = 112593, View = 4851 });
            _articleViews.Add(new ArticleView() { ArticleId = 112596, View = 698 });
            _articleViews.Add(new ArticleView() { ArticleId = 112597, View = 10590 });
            _articleViews.Add(new ArticleView() { ArticleId = 112602, View = 2438 });
            _articleViews.Add(new ArticleView() { ArticleId = 112603, View = 6897 });
            _articleViews.Add(new ArticleView() { ArticleId = 112641, View = 386 });
            _articleViews.Add(new ArticleView() { ArticleId = 112643, View = 524 });
            _articleViews.Add(new ArticleView() { ArticleId = 112644, View = 1613 });
            _articleViews.Add(new ArticleView() { ArticleId = 112258, View = 1555 });
            _articleViews.Add(new ArticleView() { ArticleId = 112262, View = 916 });
            _articleViews.Add(new ArticleView() { ArticleId = 112273, View = 923 });
            _articleViews.Add(new ArticleView() { ArticleId = 112691, View = 743 });
            _articleViews.Add(new ArticleView() { ArticleId = 112500, View = 306 });
            _articleViews.Add(new ArticleView() { ArticleId = 112504, View = 2033 });
            _articleViews.Add(new ArticleView() { ArticleId = 112509, View = 3695 });
            _articleViews.Add(new ArticleView() { ArticleId = 112529, View = 768 });
            _articleViews.Add(new ArticleView() { ArticleId = 112530, View = 238 });
            _articleViews.Add(new ArticleView() { ArticleId = 112531, View = 2034 });
            _articleViews.Add(new ArticleView() { ArticleId = 112533, View = 953 });
            _articleViews.Add(new ArticleView() { ArticleId = 112534, View = 377 });
            _articleViews.Add(new ArticleView() { ArticleId = 112540, View = 739 });
            _articleViews.Add(new ArticleView() { ArticleId = 112541, View = 436 });
            _articleViews.Add(new ArticleView() { ArticleId = 112104, View = 478 });
            _articleViews.Add(new ArticleView() { ArticleId = 112105, View = 2647 });
            _articleViews.Add(new ArticleView() { ArticleId = 112610, View = 8014 });
            _articleViews.Add(new ArticleView() { ArticleId = 112613, View = 409 });
            _articleViews.Add(new ArticleView() { ArticleId = 112693, View = 4260 });
            _articleViews.Add(new ArticleView() { ArticleId = 112701, View = 991 });
            _articleViews.Add(new ArticleView() { ArticleId = 112702, View = 1137 });
            _articleViews.Add(new ArticleView() { ArticleId = 112704, View = 1366 });
            _articleViews.Add(new ArticleView() { ArticleId = 112543, View = 1549 });
            _articleViews.Add(new ArticleView() { ArticleId = 112545, View = 1107 });
            _articleViews.Add(new ArticleView() { ArticleId = 112614, View = 7783 });
            _articleViews.Add(new ArticleView() { ArticleId = 112616, View = 4211 });
            _articleViews.Add(new ArticleView() { ArticleId = 112617, View = 968 });
            _articleViews.Add(new ArticleView() { ArticleId = 112618, View = 2391 });
            _articleViews.Add(new ArticleView() { ArticleId = 112621, View = 772 });
            _articleViews.Add(new ArticleView() { ArticleId = 112626, View = 514 });
            _articleViews.Add(new ArticleView() { ArticleId = 112705, View = 2914 });
            _articleViews.Add(new ArticleView() { ArticleId = 112708, View = 360 });
            _articleViews.Add(new ArticleView() { ArticleId = 112709, View = 257 });
            _articleViews.Add(new ArticleView() { ArticleId = 112343, View = 5719 });
            _articleViews.Add(new ArticleView() { ArticleId = 112345, View = 1433 });
            _articleViews.Add(new ArticleView() { ArticleId = 112347, View = 1961 });
            _articleViews.Add(new ArticleView() { ArticleId = 112752, View = 7387 });
            _articleViews.Add(new ArticleView() { ArticleId = 112547, View = 593 });
            _articleViews.Add(new ArticleView() { ArticleId = 112549, View = 163 });
            _articleViews.Add(new ArticleView() { ArticleId = 112551, View = 2604 });
            _articleViews.Add(new ArticleView() { ArticleId = 112645, View = 547 });
            _articleViews.Add(new ArticleView() { ArticleId = 112646, View = 1308 });
            _articleViews.Add(new ArticleView() { ArticleId = 112647, View = 1285 });
            _articleViews.Add(new ArticleView() { ArticleId = 112649, View = 1427 });
            _articleViews.Add(new ArticleView() { ArticleId = 112650, View = 3368 });
            _articleViews.Add(new ArticleView() { ArticleId = 112652, View = 882 });
            _articleViews.Add(new ArticleView() { ArticleId = 112627, View = 274 });
            _articleViews.Add(new ArticleView() { ArticleId = 112628, View = 1421 });
            _articleViews.Add(new ArticleView() { ArticleId = 112805, View = 177 });
            _articleViews.Add(new ArticleView() { ArticleId = 112654, View = 570 });
            _articleViews.Add(new ArticleView() { ArticleId = 112655, View = 431 });
            _articleViews.Add(new ArticleView() { ArticleId = 112552, View = 138 });
            _articleViews.Add(new ArticleView() { ArticleId = 112563, View = 991 });
            _articleViews.Add(new ArticleView() { ArticleId = 112567, View = 4983 });
            _articleViews.Add(new ArticleView() { ArticleId = 112568, View = 4597 });
            _articleViews.Add(new ArticleView() { ArticleId = 112572, View = 2802 });
            _articleViews.Add(new ArticleView() { ArticleId = 112756, View = 1004 });
            _articleViews.Add(new ArticleView() { ArticleId = 112757, View = 374 });
            _articleViews.Add(new ArticleView() { ArticleId = 112657, View = 3794 });
            _articleViews.Add(new ArticleView() { ArticleId = 112660, View = 23198 });
            _articleViews.Add(new ArticleView() { ArticleId = 112725, View = 7203 });
            _articleViews.Add(new ArticleView() { ArticleId = 112726, View = 80 });
            _articleViews.Add(new ArticleView() { ArticleId = 112511, View = 15413 });
            _articleViews.Add(new ArticleView() { ArticleId = 112514, View = 1622 });
            _articleViews.Add(new ArticleView() { ArticleId = 112519, View = 762 });
            _articleViews.Add(new ArticleView() { ArticleId = 112764, View = 4061 });
            _articleViews.Add(new ArticleView() { ArticleId = 112765, View = 1609 });
            _articleViews.Add(new ArticleView() { ArticleId = 112854, View = 139 });
            _articleViews.Add(new ArticleView() { ArticleId = 112855, View = 737 });
            _articleViews.Add(new ArticleView() { ArticleId = 112856, View = 634 });
            _articleViews.Add(new ArticleView() { ArticleId = 112858, View = 6240 });
            _articleViews.Add(new ArticleView() { ArticleId = 112859, View = 1357 });
            _articleViews.Add(new ArticleView() { ArticleId = 112860, View = 2062 });
            _articleViews.Add(new ArticleView() { ArticleId = 112810, View = 2530 });
            _articleViews.Add(new ArticleView() { ArticleId = 112813, View = 227 });
            _articleViews.Add(new ArticleView() { ArticleId = 112815, View = 1343 });
            _articleViews.Add(new ArticleView() { ArticleId = 112820, View = 4158 });
            _articleViews.Add(new ArticleView() { ArticleId = 112822, View = 2521 });
            _articleViews.Add(new ArticleView() { ArticleId = 112664, View = 17163 });
            _articleViews.Add(new ArticleView() { ArticleId = 112667, View = 988 });
            _articleViews.Add(new ArticleView() { ArticleId = 112670, View = 720 });
            _articleViews.Add(new ArticleView() { ArticleId = 112766, View = 601 });
            _articleViews.Add(new ArticleView() { ArticleId = 112773, View = 2516 });
            _articleViews.Add(new ArticleView() { ArticleId = 112861, View = 530 });
            _articleViews.Add(new ArticleView() { ArticleId = 112863, View = 757 });
            _articleViews.Add(new ArticleView() { ArticleId = 112727, View = 7301 });
            _articleViews.Add(new ArticleView() { ArticleId = 112728, View = 3317 });
            _articleViews.Add(new ArticleView() { ArticleId = 112730, View = 391 });
            _articleViews.Add(new ArticleView() { ArticleId = 112732, View = 221 });
            _articleViews.Add(new ArticleView() { ArticleId = 112736, View = 824 });
            _articleViews.Add(new ArticleView() { ArticleId = 112745, View = 1762 });
            _articleViews.Add(new ArticleView() { ArticleId = 112865, View = 497 });
            _articleViews.Add(new ArticleView() { ArticleId = 112868, View = 315 });
            _articleViews.Add(new ArticleView() { ArticleId = 112869, View = 1419 });
            _articleViews.Add(new ArticleView() { ArticleId = 112870, View = 184 });
            _articleViews.Add(new ArticleView() { ArticleId = 112872, View = 1018 });
            _articleViews.Add(new ArticleView() { ArticleId = 112873, View = 2 });
            _articleViews.Add(new ArticleView() { ArticleId = 112875, View = 251 });
            _articleViews.Add(new ArticleView() { ArticleId = 112777, View = 687 });
            _articleViews.Add(new ArticleView() { ArticleId = 112779, View = 1587 });
            _articleViews.Add(new ArticleView() { ArticleId = 112782, View = 1679 });
            _articleViews.Add(new ArticleView() { ArticleId = 112575, View = 208 });
            _articleViews.Add(new ArticleView() { ArticleId = 112576, View = 692 });
            _articleViews.Add(new ArticleView() { ArticleId = 112580, View = 670 });
            _articleViews.Add(new ArticleView() { ArticleId = 112903, View = 1849 });
            _articleViews.Add(new ArticleView() { ArticleId = 112904, View = 1576 });
            _articleViews.Add(new ArticleView() { ArticleId = 112905, View = 1250 });
            _articleViews.Add(new ArticleView() { ArticleId = 112906, View = 464 });
            _articleViews.Add(new ArticleView() { ArticleId = 112907, View = 220 });
            _articleViews.Add(new ArticleView() { ArticleId = 112908, View = 27781 });
            _articleViews.Add(new ArticleView() { ArticleId = 112909, View = 5238 });
            _articleViews.Add(new ArticleView() { ArticleId = 112783, View = 8876 });
            _articleViews.Add(new ArticleView() { ArticleId = 112790, View = 702 });
            _articleViews.Add(new ArticleView() { ArticleId = 112791, View = 4404 });
            _articleViews.Add(new ArticleView() { ArticleId = 112792, View = 648 });
            _articleViews.Add(new ArticleView() { ArticleId = 112793, View = 1353 });
            _articleViews.Add(new ArticleView() { ArticleId = 112796, View = 342 });
            _articleViews.Add(new ArticleView() { ArticleId = 112959, View = 98 });
            _articleViews.Add(new ArticleView() { ArticleId = 112960, View = 86 });
            _articleViews.Add(new ArticleView() { ArticleId = 112961, View = 393 });
            _articleViews.Add(new ArticleView() { ArticleId = 112962, View = 394 });
            _articleViews.Add(new ArticleView() { ArticleId = 112671, View = 1433 });
            _articleViews.Add(new ArticleView() { ArticleId = 112672, View = 2866 });
            _articleViews.Add(new ArticleView() { ArticleId = 112674, View = 1167 });
            _articleViews.Add(new ArticleView() { ArticleId = 112676, View = 11824 });
            _articleViews.Add(new ArticleView() { ArticleId = 112681, View = 951 });
            _articleViews.Add(new ArticleView() { ArticleId = 112910, View = 1375 });
            _articleViews.Add(new ArticleView() { ArticleId = 112911, View = 3123 });
            _articleViews.Add(new ArticleView() { ArticleId = 112913, View = 659 });
            _articleViews.Add(new ArticleView() { ArticleId = 112914, View = 6781 });
            _articleViews.Add(new ArticleView() { ArticleId = 112917, View = 22121 });
            _articleViews.Add(new ArticleView() { ArticleId = 112877, View = 573 });
            _articleViews.Add(new ArticleView() { ArticleId = 112878, View = 42462 });
            _articleViews.Add(new ArticleView() { ArticleId = 112881, View = 327 });
            _articleViews.Add(new ArticleView() { ArticleId = 112882, View = 265 });
            _articleViews.Add(new ArticleView() { ArticleId = 112798, View = 2766 });
            _articleViews.Add(new ArticleView() { ArticleId = 113021, View = 1480 });
            _articleViews.Add(new ArticleView() { ArticleId = 113022, View = 233 });
            _articleViews.Add(new ArticleView() { ArticleId = 113023, View = 458 });
            _articleViews.Add(new ArticleView() { ArticleId = 113024, View = 154 });
            _articleViews.Add(new ArticleView() { ArticleId = 112963, View = 49 });
            _articleViews.Add(new ArticleView() { ArticleId = 112964, View = 591 });
            _articleViews.Add(new ArticleView() { ArticleId = 112965, View = 838 });
            _articleViews.Add(new ArticleView() { ArticleId = 112966, View = 1145 });
            _articleViews.Add(new ArticleView() { ArticleId = 112967, View = 48939 });
            _articleViews.Add(new ArticleView() { ArticleId = 112825, View = 1443 });
            _articleViews.Add(new ArticleView() { ArticleId = 112831, View = 1534 });
            _articleViews.Add(new ArticleView() { ArticleId = 112832, View = 3771 });
            _articleViews.Add(new ArticleView() { ArticleId = 112918, View = 1428 });
            _articleViews.Add(new ArticleView() { ArticleId = 112919, View = 979 });
            _articleViews.Add(new ArticleView() { ArticleId = 112920, View = 281 });
            _articleViews.Add(new ArticleView() { ArticleId = 112921, View = 749 });
            _articleViews.Add(new ArticleView() { ArticleId = 112883, View = 4391 });
            _articleViews.Add(new ArticleView() { ArticleId = 112884, View = 437 });
            _articleViews.Add(new ArticleView() { ArticleId = 112885, View = 328 });
            _articleViews.Add(new ArticleView() { ArticleId = 112886, View = 230 });
            _articleViews.Add(new ArticleView() { ArticleId = 112887, View = 869 });
            _articleViews.Add(new ArticleView() { ArticleId = 112888, View = 134 });
            _articleViews.Add(new ArticleView() { ArticleId = 112923, View = 409 });
            _articleViews.Add(new ArticleView() { ArticleId = 112925, View = 990 });
            _articleViews.Add(new ArticleView() { ArticleId = 112926, View = 149 });
            _articleViews.Add(new ArticleView() { ArticleId = 112927, View = 312 });
            _articleViews.Add(new ArticleView() { ArticleId = 113025, View = 587 });
            _articleViews.Add(new ArticleView() { ArticleId = 113026, View = 4326 });
            _articleViews.Add(new ArticleView() { ArticleId = 113027, View = 2847 });
            _articleViews.Add(new ArticleView() { ArticleId = 113028, View = 575 });
            _articleViews.Add(new ArticleView() { ArticleId = 113030, View = 192 });
            _articleViews.Add(new ArticleView() { ArticleId = 113031, View = 64 });
            _articleViews.Add(new ArticleView() { ArticleId = 113032, View = 2281 });
            _articleViews.Add(new ArticleView() { ArticleId = 113033, View = 18508 });
            _articleViews.Add(new ArticleView() { ArticleId = 113034, View = 1428 });
            _articleViews.Add(new ArticleView() { ArticleId = 113035, View = 632 });
            _articleViews.Add(new ArticleView() { ArticleId = 113036, View = 689 });
            _articleViews.Add(new ArticleView() { ArticleId = 113037, View = 415 });
            _articleViews.Add(new ArticleView() { ArticleId = 113038, View = 1367 });
            _articleViews.Add(new ArticleView() { ArticleId = 113039, View = 725 });
            _articleViews.Add(new ArticleView() { ArticleId = 113040, View = 38 });
            _articleViews.Add(new ArticleView() { ArticleId = 113041, View = 241 });
            _articleViews.Add(new ArticleView() { ArticleId = 113042, View = 161 });
            _articleViews.Add(new ArticleView() { ArticleId = 113043, View = 8083 });
            _articleViews.Add(new ArticleView() { ArticleId = 113044, View = 654 });
            _articleViews.Add(new ArticleView() { ArticleId = 113045, View = 11797 });
            _articleViews.Add(new ArticleView() { ArticleId = 112106, View = 527 });
            _articleViews.Add(new ArticleView() { ArticleId = 112687, View = 6041 });
            _articleViews.Add(new ArticleView() { ArticleId = 112688, View = 262 });
            _articleViews.Add(new ArticleView() { ArticleId = 112968, View = 195 });
            _articleViews.Add(new ArticleView() { ArticleId = 112969, View = 1185 });
            _articleViews.Add(new ArticleView() { ArticleId = 112970, View = 31 });
            _articleViews.Add(new ArticleView() { ArticleId = 112889, View = 410 });
            _articleViews.Add(new ArticleView() { ArticleId = 112890, View = 52 });
            _articleViews.Add(new ArticleView() { ArticleId = 112891, View = 1919 });
            _articleViews.Add(new ArticleView() { ArticleId = 112928, View = 4475 });
            _articleViews.Add(new ArticleView() { ArticleId = 112929, View = 1129 });
            _articleViews.Add(new ArticleView() { ArticleId = 112930, View = 3366 });
            _articleViews.Add(new ArticleView() { ArticleId = 112932, View = 558 });
            _articleViews.Add(new ArticleView() { ArticleId = 112933, View = 26996 });
            _articleViews.Add(new ArticleView() { ArticleId = 112833, View = 3402 });
            _articleViews.Add(new ArticleView() { ArticleId = 112835, View = 290 });
            _articleViews.Add(new ArticleView() { ArticleId = 112836, View = 812 });
            _articleViews.Add(new ArticleView() { ArticleId = 112837, View = 200 });
            _articleViews.Add(new ArticleView() { ArticleId = 112838, View = 780 });
            _articleViews.Add(new ArticleView() { ArticleId = 112839, View = 260 });
            _articleViews.Add(new ArticleView() { ArticleId = 112840, View = 662 });
            _articleViews.Add(new ArticleView() { ArticleId = 112892, View = 74 });
            _articleViews.Add(new ArticleView() { ArticleId = 112894, View = 258 });
            _articleViews.Add(new ArticleView() { ArticleId = 113046, View = 39 });
            _articleViews.Add(new ArticleView() { ArticleId = 113047, View = 236 });
            _articleViews.Add(new ArticleView() { ArticleId = 113048, View = 213 });
            _articleViews.Add(new ArticleView() { ArticleId = 112934, View = 1290 });
            _articleViews.Add(new ArticleView() { ArticleId = 112935, View = 176 });
            _articleViews.Add(new ArticleView() { ArticleId = 112936, View = 4851 });
            _articleViews.Add(new ArticleView() { ArticleId = 112971, View = 252 });
            _articleViews.Add(new ArticleView() { ArticleId = 112841, View = 18093 });
            _articleViews.Add(new ArticleView() { ArticleId = 112842, View = 272 });
            _articleViews.Add(new ArticleView() { ArticleId = 113049, View = 969 });
            _articleViews.Add(new ArticleView() { ArticleId = 113050, View = 328 });
            _articleViews.Add(new ArticleView() { ArticleId = 112689, View = 889 });
            _articleViews.Add(new ArticleView() { ArticleId = 113086, View = 3451 });
            _articleViews.Add(new ArticleView() { ArticleId = 112937, View = 3537 });
            _articleViews.Add(new ArticleView() { ArticleId = 112938, View = 553 });
            _articleViews.Add(new ArticleView() { ArticleId = 112939, View = 327 });
            _articleViews.Add(new ArticleView() { ArticleId = 112940, View = 1096 });
            _articleViews.Add(new ArticleView() { ArticleId = 112941, View = 121 });
            _articleViews.Add(new ArticleView() { ArticleId = 112895, View = 2089 });
            _articleViews.Add(new ArticleView() { ArticleId = 112896, View = 611 });
            _articleViews.Add(new ArticleView() { ArticleId = 113051, View = 500 });
            _articleViews.Add(new ArticleView() { ArticleId = 113052, View = 108 });
            _articleViews.Add(new ArticleView() { ArticleId = 113053, View = 378 });
            _articleViews.Add(new ArticleView() { ArticleId = 113054, View = 168 });
            _articleViews.Add(new ArticleView() { ArticleId = 113055, View = 914 });
            _articleViews.Add(new ArticleView() { ArticleId = 113056, View = 737 });
            _articleViews.Add(new ArticleView() { ArticleId = 113057, View = 649 });
            _articleViews.Add(new ArticleView() { ArticleId = 113058, View = 331 });
            _articleViews.Add(new ArticleView() { ArticleId = 112942, View = 103 });
            _articleViews.Add(new ArticleView() { ArticleId = 112944, View = 155 });
            _articleViews.Add(new ArticleView() { ArticleId = 112945, View = 329 });
            _articleViews.Add(new ArticleView() { ArticleId = 112946, View = 1263 });
            _articleViews.Add(new ArticleView() { ArticleId = 112947, View = 48 });
            _articleViews.Add(new ArticleView() { ArticleId = 112948, View = 927 });
            _articleViews.Add(new ArticleView() { ArticleId = 112949, View = 387 });
            _articleViews.Add(new ArticleView() { ArticleId = 112950, View = 94 });
            _articleViews.Add(new ArticleView() { ArticleId = 112951, View = 126 });
            _articleViews.Add(new ArticleView() { ArticleId = 112953, View = 77 });
            _articleViews.Add(new ArticleView() { ArticleId = 112954, View = 389 });
            _articleViews.Add(new ArticleView() { ArticleId = 112955, View = 596 });
            _articleViews.Add(new ArticleView() { ArticleId = 112956, View = 237 });
            _articleViews.Add(new ArticleView() { ArticleId = 112957, View = 1038 });
            _articleViews.Add(new ArticleView() { ArticleId = 112897, View = 297 });
            _articleViews.Add(new ArticleView() { ArticleId = 112898, View = 47 });
            _articleViews.Add(new ArticleView() { ArticleId = 113088, View = 2367 });
            _articleViews.Add(new ArticleView() { ArticleId = 113089, View = 8500 });
            _articleViews.Add(new ArticleView() { ArticleId = 113090, View = 505 });
            _articleViews.Add(new ArticleView() { ArticleId = 113091, View = 404 });
            _articleViews.Add(new ArticleView() { ArticleId = 113092, View = 121 });
            _articleViews.Add(new ArticleView() { ArticleId = 112958, View = 35 });
            _articleViews.Add(new ArticleView() { ArticleId = 113135, View = 526 });
            _articleViews.Add(new ArticleView() { ArticleId = 113136, View = 428 });
            _articleViews.Add(new ArticleView() { ArticleId = 113137, View = 152 });
            _articleViews.Add(new ArticleView() { ArticleId = 113138, View = 263 });
            _articleViews.Add(new ArticleView() { ArticleId = 112900, View = 297 });
            _articleViews.Add(new ArticleView() { ArticleId = 112901, View = 7775 });
            _articleViews.Add(new ArticleView() { ArticleId = 113191, View = 157 });
            _articleViews.Add(new ArticleView() { ArticleId = 113059, View = 481 });
            _articleViews.Add(new ArticleView() { ArticleId = 113060, View = 121 });
            _articleViews.Add(new ArticleView() { ArticleId = 113139, View = 870 });
            _articleViews.Add(new ArticleView() { ArticleId = 113140, View = 326 });
            _articleViews.Add(new ArticleView() { ArticleId = 113141, View = 130 });
            _articleViews.Add(new ArticleView() { ArticleId = 113142, View = 26694 });
            _articleViews.Add(new ArticleView() { ArticleId = 113143, View = 24578 });
            _articleViews.Add(new ArticleView() { ArticleId = 113144, View = 539 });
            _articleViews.Add(new ArticleView() { ArticleId = 113145, View = 8111 });
            _articleViews.Add(new ArticleView() { ArticleId = 113146, View = 2841 });
            _articleViews.Add(new ArticleView() { ArticleId = 113192, View = 197 });
            _articleViews.Add(new ArticleView() { ArticleId = 113193, View = 5399 });
            _articleViews.Add(new ArticleView() { ArticleId = 113194, View = 257 });
            _articleViews.Add(new ArticleView() { ArticleId = 113195, View = 232 });
            _articleViews.Add(new ArticleView() { ArticleId = 113196, View = 377 });
            _articleViews.Add(new ArticleView() { ArticleId = 113197, View = 322 });
            _articleViews.Add(new ArticleView() { ArticleId = 113198, View = 130 });
            _articleViews.Add(new ArticleView() { ArticleId = 113199, View = 182 });
            _articleViews.Add(new ArticleView() { ArticleId = 110805, View = 7933 });
            _articleViews.Add(new ArticleView() { ArticleId = 110806, View = 5052 });
            _articleViews.Add(new ArticleView() { ArticleId = 113147, View = 16083 });
            _articleViews.Add(new ArticleView() { ArticleId = 113148, View = 216 });
            _articleViews.Add(new ArticleView() { ArticleId = 113149, View = 460 });
            _articleViews.Add(new ArticleView() { ArticleId = 113150, View = 1971 });
            _articleViews.Add(new ArticleView() { ArticleId = 113152, View = 405 });
            _articleViews.Add(new ArticleView() { ArticleId = 113155, View = 14918 });
            _articleViews.Add(new ArticleView() { ArticleId = 113156, View = 1952 });
            _articleViews.Add(new ArticleView() { ArticleId = 113158, View = 161 });
            _articleViews.Add(new ArticleView() { ArticleId = 113159, View = 4793 });
            _articleViews.Add(new ArticleView() { ArticleId = 113160, View = 180 });
            _articleViews.Add(new ArticleView() { ArticleId = 113161, View = 507 });
            _articleViews.Add(new ArticleView() { ArticleId = 113162, View = 964 });
            _articleViews.Add(new ArticleView() { ArticleId = 113164, View = 914 });
            _articleViews.Add(new ArticleView() { ArticleId = 113165, View = 13207 });
            _articleViews.Add(new ArticleView() { ArticleId = 113166, View = 246 });
            _articleViews.Add(new ArticleView() { ArticleId = 113168, View = 131 });
            _articleViews.Add(new ArticleView() { ArticleId = 113169, View = 616 });
            _articleViews.Add(new ArticleView() { ArticleId = 113170, View = 744 });
            _articleViews.Add(new ArticleView() { ArticleId = 113171, View = 90 });
            _articleViews.Add(new ArticleView() { ArticleId = 113172, View = 2201 });
            _articleViews.Add(new ArticleView() { ArticleId = 113173, View = 3100 });
            _articleViews.Add(new ArticleView() { ArticleId = 113174, View = 734 });
            _articleViews.Add(new ArticleView() { ArticleId = 113175, View = 877 });
            _articleViews.Add(new ArticleView() { ArticleId = 113176, View = 938 });
            _articleViews.Add(new ArticleView() { ArticleId = 113177, View = 1486 });
            _articleViews.Add(new ArticleView() { ArticleId = 113178, View = 277 });
            _articleViews.Add(new ArticleView() { ArticleId = 113180, View = 591 });
            _articleViews.Add(new ArticleView() { ArticleId = 113182, View = 267 });
            _articleViews.Add(new ArticleView() { ArticleId = 113183, View = 281 });
            _articleViews.Add(new ArticleView() { ArticleId = 113184, View = 992 });
            _articleViews.Add(new ArticleView() { ArticleId = 113185, View = 4642 });
            _articleViews.Add(new ArticleView() { ArticleId = 113188, View = 4680 });
            _articleViews.Add(new ArticleView() { ArticleId = 113189, View = 109 });
            _articleViews.Add(new ArticleView() { ArticleId = 113190, View = 412 });
            _articleViews.Add(new ArticleView() { ArticleId = 113250, View = 48688 });
            _articleViews.Add(new ArticleView() { ArticleId = 113251, View = 6784 });
            _articleViews.Add(new ArticleView() { ArticleId = 113252, View = 448 });
            _articleViews.Add(new ArticleView() { ArticleId = 113253, View = 581 });
            _articleViews.Add(new ArticleView() { ArticleId = 113255, View = 208 });
            _articleViews.Add(new ArticleView() { ArticleId = 113256, View = 600 });
            _articleViews.Add(new ArticleView() { ArticleId = 113257, View = 519 });
            _articleViews.Add(new ArticleView() { ArticleId = 113258, View = 109 });
            _articleViews.Add(new ArticleView() { ArticleId = 113259, View = 238 });
            _articleViews.Add(new ArticleView() { ArticleId = 113061, View = 779 });
            _articleViews.Add(new ArticleView() { ArticleId = 113062, View = 3555 });
            _articleViews.Add(new ArticleView() { ArticleId = 113063, View = 219 });
            _articleViews.Add(new ArticleView() { ArticleId = 112843, View = 356 });
            _articleViews.Add(new ArticleView() { ArticleId = 112844, View = 59 });
            _articleViews.Add(new ArticleView() { ArticleId = 112845, View = 166 });
            _articleViews.Add(new ArticleView() { ArticleId = 112846, View = 503 });
            _articleViews.Add(new ArticleView() { ArticleId = 113260, View = 621 });
            _articleViews.Add(new ArticleView() { ArticleId = 113261, View = 447 });
            _articleViews.Add(new ArticleView() { ArticleId = 113262, View = 72 });
            _articleViews.Add(new ArticleView() { ArticleId = 113263, View = 331 });
            _articleViews.Add(new ArticleView() { ArticleId = 113264, View = 20 });
            _articleViews.Add(new ArticleView() { ArticleId = 113265, View = 106 });
            _articleViews.Add(new ArticleView() { ArticleId = 113266, View = 2121 });
            _articleViews.Add(new ArticleView() { ArticleId = 113267, View = 11688 });
            _articleViews.Add(new ArticleView() { ArticleId = 113268, View = 3263 });
            _articleViews.Add(new ArticleView() { ArticleId = 113269, View = 96 });
            _articleViews.Add(new ArticleView() { ArticleId = 113270, View = 31 });
            _articleViews.Add(new ArticleView() { ArticleId = 112847, View = 54 });
            _articleViews.Add(new ArticleView() { ArticleId = 112848, View = 548 });
            _articleViews.Add(new ArticleView() { ArticleId = 113271, View = 176 });
            _articleViews.Add(new ArticleView() { ArticleId = 113272, View = 368 });
            _articleViews.Add(new ArticleView() { ArticleId = 113273, View = 425 });
            _articleViews.Add(new ArticleView() { ArticleId = 113274, View = 25 });
            _articleViews.Add(new ArticleView() { ArticleId = 113275, View = 48 });
            _articleViews.Add(new ArticleView() { ArticleId = 112850, View = 247 });
            _articleViews.Add(new ArticleView() { ArticleId = 112851, View = 9418 });
            _articleViews.Add(new ArticleView() { ArticleId = 113295, View = 1689 });
            _articleViews.Add(new ArticleView() { ArticleId = 113296, View = 476 });
            _articleViews.Add(new ArticleView() { ArticleId = 113297, View = 1940 });
            _articleViews.Add(new ArticleView() { ArticleId = 113298, View = 894 });
            _articleViews.Add(new ArticleView() { ArticleId = 113276, View = 21229 });
            _articleViews.Add(new ArticleView() { ArticleId = 113277, View = 470 });
            _articleViews.Add(new ArticleView() { ArticleId = 113093, View = 244 });
            _articleViews.Add(new ArticleView() { ArticleId = 113094, View = 23741 });
            _articleViews.Add(new ArticleView() { ArticleId = 113299, View = 562 });
            _articleViews.Add(new ArticleView() { ArticleId = 113300, View = 94 });
            _articleViews.Add(new ArticleView() { ArticleId = 113301, View = 753 });
            _articleViews.Add(new ArticleView() { ArticleId = 113302, View = 446 });
            _articleViews.Add(new ArticleView() { ArticleId = 113303, View = 747 });
            _articleViews.Add(new ArticleView() { ArticleId = 113304, View = 10760 });
            _articleViews.Add(new ArticleView() { ArticleId = 113305, View = 9142 });
            _articleViews.Add(new ArticleView() { ArticleId = 113095, View = 1283 });
            _articleViews.Add(new ArticleView() { ArticleId = 113096, View = 243 });
            _articleViews.Add(new ArticleView() { ArticleId = 113097, View = 1001 });
            _articleViews.Add(new ArticleView() { ArticleId = 113306, View = 111 });
            _articleViews.Add(new ArticleView() { ArticleId = 113307, View = 225 });
            _articleViews.Add(new ArticleView() { ArticleId = 113308, View = 341 });
            _articleViews.Add(new ArticleView() { ArticleId = 113309, View = 469 });
            _articleViews.Add(new ArticleView() { ArticleId = 113310, View = 1799 });
            _articleViews.Add(new ArticleView() { ArticleId = 113311, View = 541 });
            _articleViews.Add(new ArticleView() { ArticleId = 113312, View = 10851 });
            _articleViews.Add(new ArticleView() { ArticleId = 113313, View = 656 });
            _articleViews.Add(new ArticleView() { ArticleId = 113314, View = 260 });
            _articleViews.Add(new ArticleView() { ArticleId = 113315, View = 1182 });
            _articleViews.Add(new ArticleView() { ArticleId = 113316, View = 99 });
            _articleViews.Add(new ArticleView() { ArticleId = 113317, View = 2384 });
            _articleViews.Add(new ArticleView() { ArticleId = 113318, View = 48 });
            _articleViews.Add(new ArticleView() { ArticleId = 113098, View = 26839 });
            _articleViews.Add(new ArticleView() { ArticleId = 113099, View = 402 });
            _articleViews.Add(new ArticleView() { ArticleId = 113100, View = 3669 });
            _articleViews.Add(new ArticleView() { ArticleId = 113319, View = 298 });
            _articleViews.Add(new ArticleView() { ArticleId = 113320, View = 390 });
            _articleViews.Add(new ArticleView() { ArticleId = 113321, View = 1333 });
            _articleViews.Add(new ArticleView() { ArticleId = 113064, View = 72 });
            _articleViews.Add(new ArticleView() { ArticleId = 113065, View = 183 });
            _articleViews.Add(new ArticleView() { ArticleId = 113066, View = 5115 });
            _articleViews.Add(new ArticleView() { ArticleId = 112972, View = 1070 });
            _articleViews.Add(new ArticleView() { ArticleId = 112973, View = 163 });
            _articleViews.Add(new ArticleView() { ArticleId = 113322, View = 305 });
            _articleViews.Add(new ArticleView() { ArticleId = 113324, View = 1922 });
            _articleViews.Add(new ArticleView() { ArticleId = 113327, View = 2368 });
            _articleViews.Add(new ArticleView() { ArticleId = 113328, View = 488 });
            _articleViews.Add(new ArticleView() { ArticleId = 113330, View = 17531 });
            _articleViews.Add(new ArticleView() { ArticleId = 113331, View = 56 });
            _articleViews.Add(new ArticleView() { ArticleId = 112974, View = 13 });
            _articleViews.Add(new ArticleView() { ArticleId = 112975, View = 18 });
            _articleViews.Add(new ArticleView() { ArticleId = 113067, View = 1308 });
            _articleViews.Add(new ArticleView() { ArticleId = 113068, View = 1696 });
            _articleViews.Add(new ArticleView() { ArticleId = 113069, View = 4839 });
            _articleViews.Add(new ArticleView() { ArticleId = 113070, View = 3528 });
            _articleViews.Add(new ArticleView() { ArticleId = 113071, View = 725 });
            _articleViews.Add(new ArticleView() { ArticleId = 113072, View = 929 });
            _articleViews.Add(new ArticleView() { ArticleId = 113073, View = 1149 });
            _articleViews.Add(new ArticleView() { ArticleId = 113074, View = 41660 });
            _articleViews.Add(new ArticleView() { ArticleId = 113076, View = 649 });
            _articleViews.Add(new ArticleView() { ArticleId = 113077, View = 9125 });
            _articleViews.Add(new ArticleView() { ArticleId = 113078, View = 6486 });
            _articleViews.Add(new ArticleView() { ArticleId = 113079, View = 588 });
            _articleViews.Add(new ArticleView() { ArticleId = 113080, View = 17982 });
            _articleViews.Add(new ArticleView() { ArticleId = 113081, View = 2456 });
            _articleViews.Add(new ArticleView() { ArticleId = 113082, View = 1029 });
            _articleViews.Add(new ArticleView() { ArticleId = 113085, View = 6186 });
            _articleViews.Add(new ArticleView() { ArticleId = 113332, View = 340 });
            _articleViews.Add(new ArticleView() { ArticleId = 113333, View = 951 });
            _articleViews.Add(new ArticleView() { ArticleId = 113334, View = 8494 });
            _articleViews.Add(new ArticleView() { ArticleId = 113335, View = 267 });
            _articleViews.Add(new ArticleView() { ArticleId = 113336, View = 443 });
            _articleViews.Add(new ArticleView() { ArticleId = 113337, View = 8025 });
            _articleViews.Add(new ArticleView() { ArticleId = 113338, View = 142 });
            _articleViews.Add(new ArticleView() { ArticleId = 113339, View = 80 });
            _articleViews.Add(new ArticleView() { ArticleId = 113348, View = 725 });
            _articleViews.Add(new ArticleView() { ArticleId = 113349, View = 468 });
            _articleViews.Add(new ArticleView() { ArticleId = 113340, View = 416 });
            _articleViews.Add(new ArticleView() { ArticleId = 113341, View = 13657 });
            _articleViews.Add(new ArticleView() { ArticleId = 113343, View = 105 });
            _articleViews.Add(new ArticleView() { ArticleId = 113344, View = 59 });
            _articleViews.Add(new ArticleView() { ArticleId = 113101, View = 2522 });
            _articleViews.Add(new ArticleView() { ArticleId = 113102, View = 334 });
            _articleViews.Add(new ArticleView() { ArticleId = 113103, View = 150 });
            _articleViews.Add(new ArticleView() { ArticleId = 113104, View = 257 });
            _articleViews.Add(new ArticleView() { ArticleId = 113105, View = 2060 });
            _articleViews.Add(new ArticleView() { ArticleId = 113106, View = 121 });
            _articleViews.Add(new ArticleView() { ArticleId = 113107, View = 213 });
            _articleViews.Add(new ArticleView() { ArticleId = 113108, View = 130 });
            _articleViews.Add(new ArticleView() { ArticleId = 113109, View = 1289 });
            _articleViews.Add(new ArticleView() { ArticleId = 113110, View = 524 });
            _articleViews.Add(new ArticleView() { ArticleId = 113111, View = 929 });
            _articleViews.Add(new ArticleView() { ArticleId = 113112, View = 313 });
            _articleViews.Add(new ArticleView() { ArticleId = 113113, View = 1571 });
            _articleViews.Add(new ArticleView() { ArticleId = 113345, View = 321 });
            _articleViews.Add(new ArticleView() { ArticleId = 113346, View = 286 });
            _articleViews.Add(new ArticleView() { ArticleId = 113347, View = 439 });
            _articleViews.Add(new ArticleView() { ArticleId = 113405, View = 39 });
            _articleViews.Add(new ArticleView() { ArticleId = 113114, View = 4824 });
            _articleViews.Add(new ArticleView() { ArticleId = 113115, View = 420 });
            _articleViews.Add(new ArticleView() { ArticleId = 113116, View = 26956 });
            _articleViews.Add(new ArticleView() { ArticleId = 113117, View = 286 });
            _articleViews.Add(new ArticleView() { ArticleId = 113118, View = 694 });
            _articleViews.Add(new ArticleView() { ArticleId = 113119, View = 433 });
            _articleViews.Add(new ArticleView() { ArticleId = 113120, View = 2240 });
            _articleViews.Add(new ArticleView() { ArticleId = 113121, View = 2045 });
            _articleViews.Add(new ArticleView() { ArticleId = 113122, View = 108 });
            _articleViews.Add(new ArticleView() { ArticleId = 113123, View = 25 });
            _articleViews.Add(new ArticleView() { ArticleId = 113124, View = 214 });
            _articleViews.Add(new ArticleView() { ArticleId = 113125, View = 330 });
            _articleViews.Add(new ArticleView() { ArticleId = 113126, View = 263 });
            _articleViews.Add(new ArticleView() { ArticleId = 113127, View = 749 });
            _articleViews.Add(new ArticleView() { ArticleId = 112976, View = 5299 });
            _articleViews.Add(new ArticleView() { ArticleId = 112977, View = 195 });
            _articleViews.Add(new ArticleView() { ArticleId = 113128, View = 367 });
            _articleViews.Add(new ArticleView() { ArticleId = 113129, View = 121 });
            _articleViews.Add(new ArticleView() { ArticleId = 112978, View = 364 });
            _articleViews.Add(new ArticleView() { ArticleId = 112979, View = 202 });
            _articleViews.Add(new ArticleView() { ArticleId = 112980, View = 111 });
            _articleViews.Add(new ArticleView() { ArticleId = 113406, View = 302 });
            _articleViews.Add(new ArticleView() { ArticleId = 113407, View = 62 });
            _articleViews.Add(new ArticleView() { ArticleId = 113408, View = 103 });
            _articleViews.Add(new ArticleView() { ArticleId = 112981, View = 453 });
            _articleViews.Add(new ArticleView() { ArticleId = 112982, View = 985 });
            _articleViews.Add(new ArticleView() { ArticleId = 112983, View = 18286 });
            _articleViews.Add(new ArticleView() { ArticleId = 113409, View = 317 });
            _articleViews.Add(new ArticleView() { ArticleId = 113410, View = 202 });
            _articleViews.Add(new ArticleView() { ArticleId = 113411, View = 294 });
            _articleViews.Add(new ArticleView() { ArticleId = 113412, View = 428 });
            _articleViews.Add(new ArticleView() { ArticleId = 113200, View = 134 });
            _articleViews.Add(new ArticleView() { ArticleId = 113201, View = 145 });
            _articleViews.Add(new ArticleView() { ArticleId = 113413, View = 10436 });
            _articleViews.Add(new ArticleView() { ArticleId = 113414, View = 195 });
            _articleViews.Add(new ArticleView() { ArticleId = 113415, View = 38 });
            _articleViews.Add(new ArticleView() { ArticleId = 113202, View = 303 });
            _articleViews.Add(new ArticleView() { ArticleId = 113203, View = 943 });
            _articleViews.Add(new ArticleView() { ArticleId = 113204, View = 268 });
            _articleViews.Add(new ArticleView() { ArticleId = 113205, View = 175 });
            _articleViews.Add(new ArticleView() { ArticleId = 113206, View = 50 });
            _articleViews.Add(new ArticleView() { ArticleId = 113207, View = 597 });
            _articleViews.Add(new ArticleView() { ArticleId = 113208, View = 513 });
            _articleViews.Add(new ArticleView() { ArticleId = 113209, View = 64 });
            _articleViews.Add(new ArticleView() { ArticleId = 113210, View = 473 });
            _articleViews.Add(new ArticleView() { ArticleId = 113211, View = 334 });
            _articleViews.Add(new ArticleView() { ArticleId = 113212, View = 738 });
            _articleViews.Add(new ArticleView() { ArticleId = 113213, View = 133 });
            _articleViews.Add(new ArticleView() { ArticleId = 113214, View = 1470 });
            _articleViews.Add(new ArticleView() { ArticleId = 113215, View = 206 });
            _articleViews.Add(new ArticleView() { ArticleId = 113216, View = 110 });
            _articleViews.Add(new ArticleView() { ArticleId = 113217, View = 190 });
            _articleViews.Add(new ArticleView() { ArticleId = 113218, View = 780 });
            _articleViews.Add(new ArticleView() { ArticleId = 113219, View = 1268 });
            _articleViews.Add(new ArticleView() { ArticleId = 113220, View = 2019 });
            _articleViews.Add(new ArticleView() { ArticleId = 113221, View = 1428 });
            _articleViews.Add(new ArticleView() { ArticleId = 113222, View = 4776 });
            _articleViews.Add(new ArticleView() { ArticleId = 113223, View = 380 });
            _articleViews.Add(new ArticleView() { ArticleId = 113224, View = 24810 });
            _articleViews.Add(new ArticleView() { ArticleId = 113225, View = 2404 });
            _articleViews.Add(new ArticleView() { ArticleId = 113226, View = 9916 });
            _articleViews.Add(new ArticleView() { ArticleId = 113416, View = 907 });
            _articleViews.Add(new ArticleView() { ArticleId = 113417, View = 24 });
            _articleViews.Add(new ArticleView() { ArticleId = 113418, View = 118 });
            _articleViews.Add(new ArticleView() { ArticleId = 113419, View = 350 });
            _articleViews.Add(new ArticleView() { ArticleId = 113420, View = 8391 });
            _articleViews.Add(new ArticleView() { ArticleId = 113421, View = 477 });
            _articleViews.Add(new ArticleView() { ArticleId = 113422, View = 600 });
            _articleViews.Add(new ArticleView() { ArticleId = 113423, View = 580 });
            _articleViews.Add(new ArticleView() { ArticleId = 113424, View = 73 });
            _articleViews.Add(new ArticleView() { ArticleId = 113227, View = 7724 });
            _articleViews.Add(new ArticleView() { ArticleId = 113228, View = 867 });
            _articleViews.Add(new ArticleView() { ArticleId = 113229, View = 393 });
            _articleViews.Add(new ArticleView() { ArticleId = 113230, View = 1294 });
            _articleViews.Add(new ArticleView() { ArticleId = 113231, View = 592 });
            _articleViews.Add(new ArticleView() { ArticleId = 113233, View = 280 });
            _articleViews.Add(new ArticleView() { ArticleId = 113234, View = 750 });
            _articleViews.Add(new ArticleView() { ArticleId = 113235, View = 476 });
            _articleViews.Add(new ArticleView() { ArticleId = 113425, View = 853 });
            _articleViews.Add(new ArticleView() { ArticleId = 113426, View = 6227 });
            _articleViews.Add(new ArticleView() { ArticleId = 113427, View = 525 });
            _articleViews.Add(new ArticleView() { ArticleId = 113428, View = 2133 });
            _articleViews.Add(new ArticleView() { ArticleId = 113429, View = 1113 });
            _articleViews.Add(new ArticleView() { ArticleId = 113430, View = 929 });
            _articleViews.Add(new ArticleView() { ArticleId = 113431, View = 715 });
            _articleViews.Add(new ArticleView() { ArticleId = 113432, View = 1043 });
            _articleViews.Add(new ArticleView() { ArticleId = 113433, View = 43 });
            _articleViews.Add(new ArticleView() { ArticleId = 113434, View = 10808 });
            _articleViews.Add(new ArticleView() { ArticleId = 113278, View = 486 });
            _articleViews.Add(new ArticleView() { ArticleId = 113279, View = 430 });
            _articleViews.Add(new ArticleView() { ArticleId = 113130, View = 3332 });
            _articleViews.Add(new ArticleView() { ArticleId = 113131, View = 146 });
            _articleViews.Add(new ArticleView() { ArticleId = 112984, View = 329 });
            _articleViews.Add(new ArticleView() { ArticleId = 113132, View = 267 });
            _articleViews.Add(new ArticleView() { ArticleId = 113133, View = 15 });
            _articleViews.Add(new ArticleView() { ArticleId = 113134, View = 233 });
            _articleViews.Add(new ArticleView() { ArticleId = 112985, View = 158 });
            _articleViews.Add(new ArticleView() { ArticleId = 112986, View = 598 });
            _articleViews.Add(new ArticleView() { ArticleId = 112987, View = 149 });
            _articleViews.Add(new ArticleView() { ArticleId = 113435, View = 510 });
            _articleViews.Add(new ArticleView() { ArticleId = 113436, View = 61 });
            _articleViews.Add(new ArticleView() { ArticleId = 113437, View = 592 });
            _articleViews.Add(new ArticleView() { ArticleId = 113438, View = 1090 });
            _articleViews.Add(new ArticleView() { ArticleId = 113439, View = 53 });
            _articleViews.Add(new ArticleView() { ArticleId = 112988, View = 2381 });
            _articleViews.Add(new ArticleView() { ArticleId = 112989, View = 246 });
            _articleViews.Add(new ArticleView() { ArticleId = 112990, View = 977 });
            _articleViews.Add(new ArticleView() { ArticleId = 112992, View = 152 });
            _articleViews.Add(new ArticleView() { ArticleId = 112993, View = 483 });
            _articleViews.Add(new ArticleView() { ArticleId = 112994, View = 148 });
            _articleViews.Add(new ArticleView() { ArticleId = 113440, View = 1654 });
            _articleViews.Add(new ArticleView() { ArticleId = 113441, View = 514 });
            _articleViews.Add(new ArticleView() { ArticleId = 112995, View = 57 });
            _articleViews.Add(new ArticleView() { ArticleId = 112996, View = 258 });
            _articleViews.Add(new ArticleView() { ArticleId = 112997, View = 5208 });
            _articleViews.Add(new ArticleView() { ArticleId = 113442, View = 201 });
            _articleViews.Add(new ArticleView() { ArticleId = 113443, View = 344 });
            _articleViews.Add(new ArticleView() { ArticleId = 112998, View = 4357 });
            _articleViews.Add(new ArticleView() { ArticleId = 112999, View = 4855 });
            _articleViews.Add(new ArticleView() { ArticleId = 113000, View = 1845 });
            _articleViews.Add(new ArticleView() { ArticleId = 113001, View = 490 });
            _articleViews.Add(new ArticleView() { ArticleId = 113444, View = 1168 });
            _articleViews.Add(new ArticleView() { ArticleId = 113445, View = 577 });
            _articleViews.Add(new ArticleView() { ArticleId = 113446, View = 1234 });
            _articleViews.Add(new ArticleView() { ArticleId = 113236, View = 7127 });
            _articleViews.Add(new ArticleView() { ArticleId = 113447, View = 1622 });
            _articleViews.Add(new ArticleView() { ArticleId = 113448, View = 1020 });
            _articleViews.Add(new ArticleView() { ArticleId = 113449, View = 1536 });
            _articleViews.Add(new ArticleView() { ArticleId = 113450, View = 173 });
            _articleViews.Add(new ArticleView() { ArticleId = 113280, View = 868 });
            _articleViews.Add(new ArticleView() { ArticleId = 113281, View = 451 });
            _articleViews.Add(new ArticleView() { ArticleId = 113282, View = 442 });
            _articleViews.Add(new ArticleView() { ArticleId = 113451, View = 3191 });
            _articleViews.Add(new ArticleView() { ArticleId = 113452, View = 1275 });
            _articleViews.Add(new ArticleView() { ArticleId = 113453, View = 590 });
            _articleViews.Add(new ArticleView() { ArticleId = 113454, View = 1207 });
            _articleViews.Add(new ArticleView() { ArticleId = 113455, View = 271 });
            _articleViews.Add(new ArticleView() { ArticleId = 113237, View = 1111 });
            _articleViews.Add(new ArticleView() { ArticleId = 113238, View = 75 });
            _articleViews.Add(new ArticleView() { ArticleId = 113239, View = 730 });
            _articleViews.Add(new ArticleView() { ArticleId = 113456, View = 1086 });
            _articleViews.Add(new ArticleView() { ArticleId = 113457, View = 109 });
            _articleViews.Add(new ArticleView() { ArticleId = 113283, View = 637 });
            _articleViews.Add(new ArticleView() { ArticleId = 113284, View = 1791 });
            _articleViews.Add(new ArticleView() { ArticleId = 113285, View = 523 });
            _articleViews.Add(new ArticleView() { ArticleId = 113286, View = 308 });
            _articleViews.Add(new ArticleView() { ArticleId = 113458, View = 569 });
            _articleViews.Add(new ArticleView() { ArticleId = 113459, View = 395 });
            _articleViews.Add(new ArticleView() { ArticleId = 113460, View = 2201 });
            _articleViews.Add(new ArticleView() { ArticleId = 113524, View = 483 });
            _articleViews.Add(new ArticleView() { ArticleId = 113525, View = 604 });
            _articleViews.Add(new ArticleView() { ArticleId = 113241, View = 362 });
            _articleViews.Add(new ArticleView() { ArticleId = 113242, View = 1657 });
            _articleViews.Add(new ArticleView() { ArticleId = 113243, View = 482 });
            _articleViews.Add(new ArticleView() { ArticleId = 113351, View = 252 });
            _articleViews.Add(new ArticleView() { ArticleId = 113352, View = 274 });
            _articleViews.Add(new ArticleView() { ArticleId = 113244, View = 3879 });
            _articleViews.Add(new ArticleView() { ArticleId = 113245, View = 4272 });
            _articleViews.Add(new ArticleView() { ArticleId = 113246, View = 3273 });
            _articleViews.Add(new ArticleView() { ArticleId = 113353, View = 12803 });
            _articleViews.Add(new ArticleView() { ArticleId = 113354, View = 6886 });
            _articleViews.Add(new ArticleView() { ArticleId = 113247, View = 7549 });
            _articleViews.Add(new ArticleView() { ArticleId = 113249, View = 4017 });
            _articleViews.Add(new ArticleView() { ArticleId = 113355, View = 8 });
            _articleViews.Add(new ArticleView() { ArticleId = 113356, View = 130 });
            _articleViews.Add(new ArticleView() { ArticleId = 113287, View = 9343 });
            _articleViews.Add(new ArticleView() { ArticleId = 113288, View = 8565 });
            _articleViews.Add(new ArticleView() { ArticleId = 113357, View = 2657 });
            _articleViews.Add(new ArticleView() { ArticleId = 113358, View = 136 });
            _articleViews.Add(new ArticleView() { ArticleId = 113359, View = 464 });
            _articleViews.Add(new ArticleView() { ArticleId = 113631, View = 338 });
            _articleViews.Add(new ArticleView() { ArticleId = 113632, View = 80 });
            _articleViews.Add(new ArticleView() { ArticleId = 113633, View = 58 });
            _articleViews.Add(new ArticleView() { ArticleId = 113002, View = 1350 });
            _articleViews.Add(new ArticleView() { ArticleId = 113003, View = 600 });
            _articleViews.Add(new ArticleView() { ArticleId = 113004, View = 852 });
            _articleViews.Add(new ArticleView() { ArticleId = 113005, View = 770 });
            _articleViews.Add(new ArticleView() { ArticleId = 113289, View = 625 });
            _articleViews.Add(new ArticleView() { ArticleId = 113290, View = 920 });
            _articleViews.Add(new ArticleView() { ArticleId = 113360, View = 45 });
            _articleViews.Add(new ArticleView() { ArticleId = 113576, View = 163 });
            _articleViews.Add(new ArticleView() { ArticleId = 113577, View = 218 });
            _articleViews.Add(new ArticleView() { ArticleId = 113006, View = 1963 });
            _articleViews.Add(new ArticleView() { ArticleId = 113007, View = 589 });
            _articleViews.Add(new ArticleView() { ArticleId = 113008, View = 12994 });
            _articleViews.Add(new ArticleView() { ArticleId = 113009, View = 1447 });
            _articleViews.Add(new ArticleView() { ArticleId = 113010, View = 705 });
            _articleViews.Add(new ArticleView() { ArticleId = 113011, View = 814 });
            _articleViews.Add(new ArticleView() { ArticleId = 113012, View = 755 });
            _articleViews.Add(new ArticleView() { ArticleId = 113361, View = 50273 });
            _articleViews.Add(new ArticleView() { ArticleId = 113362, View = 55509 });
            _articleViews.Add(new ArticleView() { ArticleId = 113363, View = 469 });
            _articleViews.Add(new ArticleView() { ArticleId = 113291, View = 1286 });
            _articleViews.Add(new ArticleView() { ArticleId = 113292, View = 1811 });
            _articleViews.Add(new ArticleView() { ArticleId = 113634, View = 222 });
            _articleViews.Add(new ArticleView() { ArticleId = 113635, View = 2114 });
            _articleViews.Add(new ArticleView() { ArticleId = 113636, View = 56 });
            _articleViews.Add(new ArticleView() { ArticleId = 113637, View = 2157 });
            _articleViews.Add(new ArticleView() { ArticleId = 113638, View = 1237 });
            _articleViews.Add(new ArticleView() { ArticleId = 113639, View = 3017 });
            _articleViews.Add(new ArticleView() { ArticleId = 113364, View = 49 });
            _articleViews.Add(new ArticleView() { ArticleId = 113365, View = 68 });
            _articleViews.Add(new ArticleView() { ArticleId = 113366, View = 987 });
            _articleViews.Add(new ArticleView() { ArticleId = 113367, View = 773 });
            _articleViews.Add(new ArticleView() { ArticleId = 113368, View = 301 });
            _articleViews.Add(new ArticleView() { ArticleId = 113369, View = 562 });
            _articleViews.Add(new ArticleView() { ArticleId = 113526, View = 1157 });
            _articleViews.Add(new ArticleView() { ArticleId = 113527, View = 1277 });
            _articleViews.Add(new ArticleView() { ArticleId = 113370, View = 1230 });
            _articleViews.Add(new ArticleView() { ArticleId = 113371, View = 8212 });
            _articleViews.Add(new ArticleView() { ArticleId = 113578, View = 685 });
            _articleViews.Add(new ArticleView() { ArticleId = 113579, View = 108 });
            _articleViews.Add(new ArticleView() { ArticleId = 113372, View = 980 });
            _articleViews.Add(new ArticleView() { ArticleId = 113373, View = 496 });
            _articleViews.Add(new ArticleView() { ArticleId = 113374, View = 311 });
            _articleViews.Add(new ArticleView() { ArticleId = 113375, View = 1366 });
            _articleViews.Add(new ArticleView() { ArticleId = 113376, View = 484 });
            _articleViews.Add(new ArticleView() { ArticleId = 113377, View = 938 });
            _articleViews.Add(new ArticleView() { ArticleId = 113378, View = 387 });
            _articleViews.Add(new ArticleView() { ArticleId = 113379, View = 3997 });
            _articleViews.Add(new ArticleView() { ArticleId = 113380, View = 2149 });
            _articleViews.Add(new ArticleView() { ArticleId = 113640, View = 5 });
            _articleViews.Add(new ArticleView() { ArticleId = 113641, View = 2305 });
            _articleViews.Add(new ArticleView() { ArticleId = 113381, View = 664 });
            _articleViews.Add(new ArticleView() { ArticleId = 113382, View = 7039 });
            _articleViews.Add(new ArticleView() { ArticleId = 113293, View = 733 });
            _articleViews.Add(new ArticleView() { ArticleId = 113013, View = 204 });
            _articleViews.Add(new ArticleView() { ArticleId = 113014, View = 1516 });
            _articleViews.Add(new ArticleView() { ArticleId = 113015, View = 307 });
            _articleViews.Add(new ArticleView() { ArticleId = 113642, View = 722 });
            _articleViews.Add(new ArticleView() { ArticleId = 113643, View = 55 });
            _articleViews.Add(new ArticleView() { ArticleId = 113016, View = 317 });
            _articleViews.Add(new ArticleView() { ArticleId = 113017, View = 298 });
            _articleViews.Add(new ArticleView() { ArticleId = 113383, View = 808 });
            _articleViews.Add(new ArticleView() { ArticleId = 113384, View = 2851 });
            _articleViews.Add(new ArticleView() { ArticleId = 113385, View = 502 });
            _articleViews.Add(new ArticleView() { ArticleId = 113018, View = 1332 });
            _articleViews.Add(new ArticleView() { ArticleId = 113019, View = 446 });
            _articleViews.Add(new ArticleView() { ArticleId = 113528, View = 1304 });
            _articleViews.Add(new ArticleView() { ArticleId = 113529, View = 273 });
            _articleViews.Add(new ArticleView() { ArticleId = 113530, View = 716 });
            _articleViews.Add(new ArticleView() { ArticleId = 113531, View = 13459 });
            _articleViews.Add(new ArticleView() { ArticleId = 113532, View = 22150 });
            _articleViews.Add(new ArticleView() { ArticleId = 113386, View = 9378 });
            _articleViews.Add(new ArticleView() { ArticleId = 113387, View = 1323 });
            _articleViews.Add(new ArticleView() { ArticleId = 113533, View = 471 });
            _articleViews.Add(new ArticleView() { ArticleId = 113534, View = 5373 });
            _articleViews.Add(new ArticleView() { ArticleId = 113535, View = 13706 });
            _articleViews.Add(new ArticleView() { ArticleId = 113536, View = 2235 });
            _articleViews.Add(new ArticleView() { ArticleId = 113020, View = 1129 });
            _articleViews.Add(new ArticleView() { ArticleId = 113644, View = 98 });
            _articleViews.Add(new ArticleView() { ArticleId = 113645, View = 45 });
            _articleViews.Add(new ArticleView() { ArticleId = 113646, View = 2 });
            _articleViews.Add(new ArticleView() { ArticleId = 113580, View = 26995 });
            _articleViews.Add(new ArticleView() { ArticleId = 113581, View = 117 });
            _articleViews.Add(new ArticleView() { ArticleId = 113461, View = 1864 });
            _articleViews.Add(new ArticleView() { ArticleId = 113462, View = 663 });
            _articleViews.Add(new ArticleView() { ArticleId = 113464, View = 368 });
            _articleViews.Add(new ArticleView() { ArticleId = 113465, View = 222 });
            _articleViews.Add(new ArticleView() { ArticleId = 113582, View = 193 });
            _articleViews.Add(new ArticleView() { ArticleId = 113583, View = 37 });
            _articleViews.Add(new ArticleView() { ArticleId = 113466, View = 393 });
            _articleViews.Add(new ArticleView() { ArticleId = 113467, View = 412 });
            _articleViews.Add(new ArticleView() { ArticleId = 113468, View = 51 });
            _articleViews.Add(new ArticleView() { ArticleId = 113469, View = 550 });
            _articleViews.Add(new ArticleView() { ArticleId = 113470, View = 313 });
            _articleViews.Add(new ArticleView() { ArticleId = 113471, View = 46 });
            _articleViews.Add(new ArticleView() { ArticleId = 113584, View = 173 });
            _articleViews.Add(new ArticleView() { ArticleId = 113585, View = 11726 });
            _articleViews.Add(new ArticleView() { ArticleId = 113647, View = 26 });
            _articleViews.Add(new ArticleView() { ArticleId = 113648, View = 140 });
            _articleViews.Add(new ArticleView() { ArticleId = 113537, View = 288 });
            _articleViews.Add(new ArticleView() { ArticleId = 113538, View = 209 });
            _articleViews.Add(new ArticleView() { ArticleId = 113539, View = 467 });
            _articleViews.Add(new ArticleView() { ArticleId = 113586, View = 1688 });
            _articleViews.Add(new ArticleView() { ArticleId = 113587, View = 227 });
            _articleViews.Add(new ArticleView() { ArticleId = 113540, View = 187 });
            _articleViews.Add(new ArticleView() { ArticleId = 113541, View = 253 });
            _articleViews.Add(new ArticleView() { ArticleId = 113542, View = 230 });
            _articleViews.Add(new ArticleView() { ArticleId = 113543, View = 197 });
            _articleViews.Add(new ArticleView() { ArticleId = 113544, View = 73 });
            _articleViews.Add(new ArticleView() { ArticleId = 113546, View = 232 });
            _articleViews.Add(new ArticleView() { ArticleId = 113588, View = 8370 });
            _articleViews.Add(new ArticleView() { ArticleId = 113589, View = 242 });
            _articleViews.Add(new ArticleView() { ArticleId = 113590, View = 23 });
            _articleViews.Add(new ArticleView() { ArticleId = 113547, View = 260 });
            _articleViews.Add(new ArticleView() { ArticleId = 113548, View = 316 });
            _articleViews.Add(new ArticleView() { ArticleId = 113549, View = 1448 });
            _articleViews.Add(new ArticleView() { ArticleId = 113550, View = 184 });
            _articleViews.Add(new ArticleView() { ArticleId = 113472, View = 761 });
            _articleViews.Add(new ArticleView() { ArticleId = 113473, View = 1239 });
            _articleViews.Add(new ArticleView() { ArticleId = 113474, View = 2054 });
            _articleViews.Add(new ArticleView() { ArticleId = 113475, View = 264 });
            _articleViews.Add(new ArticleView() { ArticleId = 113476, View = 1969 });
            _articleViews.Add(new ArticleView() { ArticleId = 113477, View = 188 });
            _articleViews.Add(new ArticleView() { ArticleId = 113591, View = 1239 });
            _articleViews.Add(new ArticleView() { ArticleId = 113592, View = 18 });
            _articleViews.Add(new ArticleView() { ArticleId = 113551, View = 5832 });
            _articleViews.Add(new ArticleView() { ArticleId = 113552, View = 58 });
            _articleViews.Add(new ArticleView() { ArticleId = 113553, View = 546 });
            _articleViews.Add(new ArticleView() { ArticleId = 113554, View = 1778 });
            _articleViews.Add(new ArticleView() { ArticleId = 113555, View = 1311 });
            _articleViews.Add(new ArticleView() { ArticleId = 113556, View = 116 });
            _articleViews.Add(new ArticleView() { ArticleId = 113557, View = 400 });
            _articleViews.Add(new ArticleView() { ArticleId = 113558, View = 507 });
            _articleViews.Add(new ArticleView() { ArticleId = 113649, View = 346 });
            _articleViews.Add(new ArticleView() { ArticleId = 113650, View = 33 });
            _articleViews.Add(new ArticleView() { ArticleId = 113593, View = 769 });
            _articleViews.Add(new ArticleView() { ArticleId = 113594, View = 8668 });
            _articleViews.Add(new ArticleView() { ArticleId = 113595, View = 38369 });
            _articleViews.Add(new ArticleView() { ArticleId = 113596, View = 5410 });
            _articleViews.Add(new ArticleView() { ArticleId = 113597, View = 214 });
            _articleViews.Add(new ArticleView() { ArticleId = 113651, View = 42213 });
            _articleViews.Add(new ArticleView() { ArticleId = 113652, View = 96 });
            _articleViews.Add(new ArticleView() { ArticleId = 113599, View = 208 });
            _articleViews.Add(new ArticleView() { ArticleId = 113600, View = 2642 });
            _articleViews.Add(new ArticleView() { ArticleId = 113601, View = 224 });
            _articleViews.Add(new ArticleView() { ArticleId = 113602, View = 239 });
            _articleViews.Add(new ArticleView() { ArticleId = 113604, View = 384 });
            _articleViews.Add(new ArticleView() { ArticleId = 113653, View = 112 });
            _articleViews.Add(new ArticleView() { ArticleId = 113654, View = 1089 });
            _articleViews.Add(new ArticleView() { ArticleId = 113605, View = 20 });
            _articleViews.Add(new ArticleView() { ArticleId = 113606, View = 330 });
            _articleViews.Add(new ArticleView() { ArticleId = 113607, View = 42 });
            _articleViews.Add(new ArticleView() { ArticleId = 113608, View = 106 });
            _articleViews.Add(new ArticleView() { ArticleId = 113609, View = 120 });
            _articleViews.Add(new ArticleView() { ArticleId = 113610, View = 2593 });
            _articleViews.Add(new ArticleView() { ArticleId = 113611, View = 327 });
            _articleViews.Add(new ArticleView() { ArticleId = 113612, View = 975 });
            _articleViews.Add(new ArticleView() { ArticleId = 113613, View = 427 });
            _articleViews.Add(new ArticleView() { ArticleId = 113614, View = 165 });
            _articleViews.Add(new ArticleView() { ArticleId = 113655, View = 101 });
            _articleViews.Add(new ArticleView() { ArticleId = 113656, View = 45 });
            _articleViews.Add(new ArticleView() { ArticleId = 113657, View = 25 });
            _articleViews.Add(new ArticleView() { ArticleId = 113658, View = 722 });
            _articleViews.Add(new ArticleView() { ArticleId = 113659, View = 336 });
            _articleViews.Add(new ArticleView() { ArticleId = 113660, View = 19 });
            _articleViews.Add(new ArticleView() { ArticleId = 113478, View = 1872 });
            _articleViews.Add(new ArticleView() { ArticleId = 113479, View = 322 });
            _articleViews.Add(new ArticleView() { ArticleId = 113480, View = 89 });
            _articleViews.Add(new ArticleView() { ArticleId = 113481, View = 172 });
            _articleViews.Add(new ArticleView() { ArticleId = 113482, View = 1240 });
            _articleViews.Add(new ArticleView() { ArticleId = 113483, View = 1845 });
            _articleViews.Add(new ArticleView() { ArticleId = 113484, View = 681 });
            _articleViews.Add(new ArticleView() { ArticleId = 113485, View = 283 });
            _articleViews.Add(new ArticleView() { ArticleId = 113615, View = 1710 });
            _articleViews.Add(new ArticleView() { ArticleId = 113616, View = 6 });
            _articleViews.Add(new ArticleView() { ArticleId = 113617, View = 304 });
            _articleViews.Add(new ArticleView() { ArticleId = 113618, View = 7114 });
            _articleViews.Add(new ArticleView() { ArticleId = 113619, View = 1354 });
            _articleViews.Add(new ArticleView() { ArticleId = 113388, View = 16035 });
            _articleViews.Add(new ArticleView() { ArticleId = 113389, View = 825 });
            _articleViews.Add(new ArticleView() { ArticleId = 113620, View = 328 });
            _articleViews.Add(new ArticleView() { ArticleId = 113621, View = 696 });
            _articleViews.Add(new ArticleView() { ArticleId = 113622, View = 2810 });
            _articleViews.Add(new ArticleView() { ArticleId = 113390, View = 9908 });
            _articleViews.Add(new ArticleView() { ArticleId = 113391, View = 8102 });
            _articleViews.Add(new ArticleView() { ArticleId = 113393, View = 463 });
            _articleViews.Add(new ArticleView() { ArticleId = 113394, View = 553 });
            _articleViews.Add(new ArticleView() { ArticleId = 113395, View = 2074 });
            _articleViews.Add(new ArticleView() { ArticleId = 113559, View = 468 });
            _articleViews.Add(new ArticleView() { ArticleId = 113560, View = 2581 });
            _articleViews.Add(new ArticleView() { ArticleId = 113561, View = 2270 });
            _articleViews.Add(new ArticleView() { ArticleId = 113562, View = 3865 });
            _articleViews.Add(new ArticleView() { ArticleId = 113396, View = 1572 });
            _articleViews.Add(new ArticleView() { ArticleId = 113397, View = 14389 });
            _articleViews.Add(new ArticleView() { ArticleId = 113398, View = 22170 });
            _articleViews.Add(new ArticleView() { ArticleId = 113399, View = 314 });
            _articleViews.Add(new ArticleView() { ArticleId = 113400, View = 5132 });
            _articleViews.Add(new ArticleView() { ArticleId = 113401, View = 547 });
            _articleViews.Add(new ArticleView() { ArticleId = 113402, View = 1143 });
            _articleViews.Add(new ArticleView() { ArticleId = 113563, View = 297 });
            _articleViews.Add(new ArticleView() { ArticleId = 113564, View = 621 });
            _articleViews.Add(new ArticleView() { ArticleId = 113403, View = 2091 });
            _articleViews.Add(new ArticleView() { ArticleId = 113404, View = 294 });
            _articleViews.Add(new ArticleView() { ArticleId = 113486, View = 52 });
            _articleViews.Add(new ArticleView() { ArticleId = 113487, View = 1424 });
            _articleViews.Add(new ArticleView() { ArticleId = 113623, View = 1031 });
            _articleViews.Add(new ArticleView() { ArticleId = 113624, View = 534 });
            _articleViews.Add(new ArticleView() { ArticleId = 113625, View = 11 });
            _articleViews.Add(new ArticleView() { ArticleId = 113488, View = 213 });
            _articleViews.Add(new ArticleView() { ArticleId = 113489, View = 520 });
            _articleViews.Add(new ArticleView() { ArticleId = 113490, View = 618 });
            _articleViews.Add(new ArticleView() { ArticleId = 113491, View = 210 });
            _articleViews.Add(new ArticleView() { ArticleId = 113492, View = 1817 });
            _articleViews.Add(new ArticleView() { ArticleId = 113493, View = 53 });
            _articleViews.Add(new ArticleView() { ArticleId = 113494, View = 469 });
            _articleViews.Add(new ArticleView() { ArticleId = 113495, View = 154 });
            _articleViews.Add(new ArticleView() { ArticleId = 113496, View = 76 });
            _articleViews.Add(new ArticleView() { ArticleId = 113497, View = 339 });
            _articleViews.Add(new ArticleView() { ArticleId = 113498, View = 1372 });
            _articleViews.Add(new ArticleView() { ArticleId = 113499, View = 1714 });
            _articleViews.Add(new ArticleView() { ArticleId = 113500, View = 2240 });
            _articleViews.Add(new ArticleView() { ArticleId = 113501, View = 346 });
            _articleViews.Add(new ArticleView() { ArticleId = 113626, View = 35204 });
            _articleViews.Add(new ArticleView() { ArticleId = 113627, View = 131 });
            _articleViews.Add(new ArticleView() { ArticleId = 113628, View = 3162 });
            _articleViews.Add(new ArticleView() { ArticleId = 113629, View = 61 });
            _articleViews.Add(new ArticleView() { ArticleId = 113630, View = 308 });
            _articleViews.Add(new ArticleView() { ArticleId = 113502, View = 356 });
            _articleViews.Add(new ArticleView() { ArticleId = 113503, View = 669 });
            _articleViews.Add(new ArticleView() { ArticleId = 113504, View = 1051 });
            _articleViews.Add(new ArticleView() { ArticleId = 113505, View = 2167 });
            _articleViews.Add(new ArticleView() { ArticleId = 113506, View = 10050 });
            _articleViews.Add(new ArticleView() { ArticleId = 113507, View = 1434 });
            _articleViews.Add(new ArticleView() { ArticleId = 113508, View = 15122 });
            _articleViews.Add(new ArticleView() { ArticleId = 113509, View = 15342 });
            _articleViews.Add(new ArticleView() { ArticleId = 113510, View = 3959 });
            _articleViews.Add(new ArticleView() { ArticleId = 113511, View = 14539 });
            _articleViews.Add(new ArticleView() { ArticleId = 113514, View = 919 });
            _articleViews.Add(new ArticleView() { ArticleId = 113515, View = 15 });
            _articleViews.Add(new ArticleView() { ArticleId = 113516, View = 1176 });
            _articleViews.Add(new ArticleView() { ArticleId = 113517, View = 251 });
            _articleViews.Add(new ArticleView() { ArticleId = 113518, View = 1569 });
            _articleViews.Add(new ArticleView() { ArticleId = 113519, View = 1463 });
            _articleViews.Add(new ArticleView() { ArticleId = 113520, View = 18498 });
            _articleViews.Add(new ArticleView() { ArticleId = 113521, View = 8395 });
            _articleViews.Add(new ArticleView() { ArticleId = 113522, View = 2119 });
            _articleViews.Add(new ArticleView() { ArticleId = 113661, View = 5175 });
            _articleViews.Add(new ArticleView() { ArticleId = 113662, View = 1837 });
            _articleViews.Add(new ArticleView() { ArticleId = 113663, View = 5562 });
            _articleViews.Add(new ArticleView() { ArticleId = 110807, View = 1471 });
            _articleViews.Add(new ArticleView() { ArticleId = 110815, View = 5165 });
            _articleViews.Add(new ArticleView() { ArticleId = 113565, View = 1334 });
            _articleViews.Add(new ArticleView() { ArticleId = 113566, View = 93 });
            _articleViews.Add(new ArticleView() { ArticleId = 113567, View = 43 });
            _articleViews.Add(new ArticleView() { ArticleId = 113664, View = 872 });
            _articleViews.Add(new ArticleView() { ArticleId = 113665, View = 9 });
            _articleViews.Add(new ArticleView() { ArticleId = 113568, View = 3469 });
            _articleViews.Add(new ArticleView() { ArticleId = 113569, View = 496 });
            _articleViews.Add(new ArticleView() { ArticleId = 113570, View = 150 });
            _articleViews.Add(new ArticleView() { ArticleId = 113571, View = 1439 });
            _articleViews.Add(new ArticleView() { ArticleId = 113572, View = 18 });
            _articleViews.Add(new ArticleView() { ArticleId = 113573, View = 15 });
            _articleViews.Add(new ArticleView() { ArticleId = 113574, View = 124 });
            _articleViews.Add(new ArticleView() { ArticleId = 113575, View = 1368 });
            _articleViews.Add(new ArticleView() { ArticleId = 113666, View = 1328 });
            _articleViews.Add(new ArticleView() { ArticleId = 113667, View = 122 });
            _articleViews.Add(new ArticleView() { ArticleId = 113668, View = 1741 });
            _articleViews.Add(new ArticleView() { ArticleId = 113669, View = 1022 });
            _articleViews.Add(new ArticleView() { ArticleId = 113671, View = 6366 });
            _articleViews.Add(new ArticleView() { ArticleId = 110817, View = 3237 });
            _articleViews.Add(new ArticleView() { ArticleId = 110819, View = 974 });
            _articleViews.Add(new ArticleView() { ArticleId = 110820, View = 407 });
            _articleViews.Add(new ArticleView() { ArticleId = 110821, View = 1855 });
            _articleViews.Add(new ArticleView() { ArticleId = 113672, View = 2123 });
            _articleViews.Add(new ArticleView() { ArticleId = 113674, View = 391 });
            _articleViews.Add(new ArticleView() { ArticleId = 113675, View = 6109 });
            _articleViews.Add(new ArticleView() { ArticleId = 113676, View = 414 });
            _articleViews.Add(new ArticleView() { ArticleId = 113677, View = 374 });
            _articleViews.Add(new ArticleView() { ArticleId = 113678, View = 7 });



        }

        private void InitializeReporter()
        {
            _reporters = new List<Reporter>();
            //_reporters.Add(new Reporter() { Id = 20000, Name = "សំ សុភារិន្ទ", Province = "Translator", Order = 23 });
            //_reporters.Add(new Reporter() { Id = 24000, Name = "ហេង ដេត", Province = "Translator", Order = 23 });
            //_reporters.Add(new Reporter() { Id = 24002, Name = "ខាន់ សុខឃន", Province = "Translator", Order = 23 });
            //_reporters.Add(new Reporter() { Id = 24003, Name = "ឈិត គឹមឆុន", Province = "Translator", Order = 23 });
            //_reporters.Add(new Reporter() { Id = 24004, Name = "ម៉ុក សេង​", Province = "Translator", Order = 23 });
            //_reporters.Add(new Reporter() { Id = 24005, Name = "តាទ្រី", Province = "Translator", Order = 23 });
            //_reporters.Add(new Reporter() { Id = 20006, Name = "អុីវ វិចិត្រា", Province = "Translator", Order = 23 });
            //_reporters.Add(new Reporter() { Id = 60010, Name = "ជួប សុខជា", Province = "Translator", Order = 23 });
            //_reporters.Add(new Reporter() { Id = 32000, Name = "សៅ សាម៉ន", Province = "Translator", Order = 23 });
            //_reporters.Add(new Reporter() { Id = 00000, Name = "ពាណិជ្ជកម្ម", Province = "Translator", Order = 23 });
            //_reporters.Add(new Reporter() { Id = 88888, Name = "សហការី", Province = "Translator", Order = 23 });
            _reporters.Add(new Reporter() { Id = 27000, Name = "ឃឹម ប៊ុនណាក់", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27002, Name = "ម៉េង ធា", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27003, Name = "យន់  ស៊ីថា ", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27004, Name = "អ៊ុំ   សំអូន", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27005, Name = "ទុយ ប៊ុនរី", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27006, Name = "ម៉ម  វ៉ាន", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27007, Name = "អ៊ូ    ច័ន្ទថា ", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27008, Name = "ចន  ណារិទ្ធ", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27009, Name = "ស.សុខុម (អ៊ាង ម៉េងឡេង)", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27010, Name = "សុខ  សារ៉ាយ", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27011, Name = "អ៊ុន  សុគន្ធា", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27012, Name = "យឹម ឈឿន", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27013, Name = "​សុវណ្ណ សុខា", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27014, Name = "អ៊ាង  ប៊ុនរិទ្ធ", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27015, Name = "សុខ  វាសនា", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27016, Name = "សុវណ្ណណែត", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27018, Name = "​សុវណ្ណ រិទ្ធី", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27019, Name = "ផាត់ តារារដ្ឋ", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27020, Name = "សុខ លក្ខណ៍", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27021, Name = "ខាត់ សាមឿន", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 27022, Name = "ឡុង សារ៉េត", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 57000, Name = "ភី ផល", Province = "Phnom Penh", Order = 1 });
            _reporters.Add(new Reporter() { Id = 29000, Name = "ស៊ឹម សាន់", Province = "Kandal", Order = 2 });
            _reporters.Add(new Reporter() { Id = 29001, Name = "ឈឺន ធា", Province = "Kandal", Order = 2 });
            _reporters.Add(new Reporter() { Id = 29002, Name = "កេត វណ្ណៈ", Province = "Kandal", Order = 2 });
            _reporters.Add(new Reporter() { Id = 63004, Name = "ទឹម  សុិន", Province = "Kandal", Order = 2 });
            _reporters.Add(new Reporter() { Id = 29043, Name = "វង្ស សន", Province = "Kampong Speu", Order = 3 });
            _reporters.Add(new Reporter() { Id = 29004, Name = "ឈុន សារុំ", Province = "SHV", Order = 4 });
            _reporters.Add(new Reporter() { Id = 29005, Name = "ហ៊ុយ  ចាន់ធួន", Province = "SHV", Order = 4 });
            _reporters.Add(new Reporter() { Id = 29006, Name = "ឈុន គឹមហៃ", Province = "SHV", Order = 4 });
            _reporters.Add(new Reporter() { Id = 29007, Name = "រស់ ភីណា", Province = "SHV", Order = 4 });
            _reporters.Add(new Reporter() { Id = 29009, Name = "ម៉ៅ គឹមហេង", Province = "Koh Kong", Order = 5 });
            _reporters.Add(new Reporter() { Id = 29008, Name = "កែវ ឆាត", Province = "Kampot", Order = 6 });
            _reporters.Add(new Reporter() { Id = 29036, Name = "ប្រាក់ សារិទ្ធ", Province = "Kampot", Order = 6 });
            _reporters.Add(new Reporter() { Id = 29010, Name = "ឆោម ពិសម័យ", Province = "Battambang", Order = 7 });
            _reporters.Add(new Reporter() { Id = 29011, Name = "ហួត សុងហាក់", Province = "Battambang", Order = 7 });
            _reporters.Add(new Reporter() { Id = 29014, Name = "ពេជ្រ សុវណ្ណារ៉ា", Province = "Banteay Meanchey", Order = 8 });
            _reporters.Add(new Reporter() { Id = 29015, Name = "ម៉ុត សារុន ", Province = "Banteay Meanchey", Order = 8 });
            _reporters.Add(new Reporter() { Id = 29016, Name = "ណុប រក្សា", Province = "Banteay Meanchey", Order = 8 });
            _reporters.Add(new Reporter() { Id = 29017, Name = "ប៊ុន ធឿន", Province = "Banteay Meanchey", Order = 8 });
            _reporters.Add(new Reporter() { Id = 29027, Name = "ឱម សារឿន", Province = "Odor Meanchey", Order = 9 });
            _reporters.Add(new Reporter() { Id = 29013, Name = "ពៅ សុខហ៊ាន", Province = "Kampong Chhnang", Order = 10 });
            _reporters.Add(new Reporter() { Id = 29022, Name = "ស៊ិន បូ", Province = "Rattanak Kiri", Order = 11 });
            _reporters.Add(new Reporter() { Id = 29023, Name = "ផង់ វិន", Province = "Rattanak Kiri", Order = 11 });
            _reporters.Add(new Reporter() { Id = 29018, Name = "ស៊ុម ស៊ីថាវរី", Province = "Kampong Thom", Order = 12 });
            _reporters.Add(new Reporter() { Id = 29042, Name = "ឆាយ រក្សា", Province = "Kampong Thom", Order = 12 });
            _reporters.Add(new Reporter() { Id = 29033, Name = "ស៊ឹម សំណាង", Province = "Siem Reap", Order = 13 });
            _reporters.Add(new Reporter() { Id = 29044, Name = "ចាន់ រស្មី", Province = "Siem Reap", Order = 13 });
            _reporters.Add(new Reporter() { Id = 29041, Name = "ទ្រី វិច្ឆិកា", Province = "Preah Vihea", Order = 14 });
            _reporters.Add(new Reporter() { Id = 29046, Name = "សុខ ប៊ុនថត", Province = "Preah Vihea", Order = 14 });
            _reporters.Add(new Reporter() { Id = 29021, Name = "ប៊ុន លីនណា", Province = "Kratie", Order = 15 });
            _reporters.Add(new Reporter() { Id = 29012, Name = "រ៉ូ សាលី", Province = "Steung Treng", Order = 16 });
            _reporters.Add(new Reporter() { Id = 29034, Name = "ពុយ វឌ្ឍនា", Province = "Steung Treng", Order = 16 });
            _reporters.Add(new Reporter() { Id = 29039, Name = "នីយ៉ា", Province = "Pailin", Order = 17 });
            _reporters.Add(new Reporter() { Id = 29020, Name = "យ៉ាន់ ចាន់ថេត", Province = "Kampong Cham", Order = 18 });
            _reporters.Add(new Reporter() { Id = 29026, Name = "សុំ ពៅពិសិដ្ឋ", Province = "Prey Veng", Order = 19 });
            _reporters.Add(new Reporter() { Id = 88888, Name = "ទន់ សាមុត", Province = "Prey Veng", Order = 19 });
            _reporters.Add(new Reporter() { Id = 29025, Name = "វ៉ាន់ ប៊ុនធឿន", Province = "Svay Rieng", Order = 20 });
            _reporters.Add(new Reporter() { Id = 29030, Name = "សេង សុធា", Province = "Takeo", Order = 21 });
            _reporters.Add(new Reporter() { Id = 29031, Name = "សែម សុគន្ធ", Province = "Posat", Order = 22 });
        }

        private void InitializeAuthors()
        {
            _authors = new List<Author>();
            _authors.Add(new Author() { Id = 1, Name = "Admin" });
            _authors.Add(new Author() { Id = 2, Name = "thy" });
            _authors.Add(new Author() { Id = 3, Name = "pcdev" });
            _authors.Add(new Author() { Id = 4, Name = "piseth" });
            _authors.Add(new Author() { Id = 10, Name = "samorn" });
            _authors.Add(new Author() { Id = 12, Name = "bross" });
            _authors.Add(new Author() { Id = 17, Name = "thyadmin" });
            _authors.Add(new Author() { Id = 22, Name = "own" });
            _authors.Add(new Author() { Id = 23, Name = "sokly" });
            _authors.Add(new Author() { Id = 24, Name = "chhon" });
            _authors.Add(new Author() { Id = 25, Name = "ypiseth" });
            _authors.Add(new Author() { Id = 27, Name = "Sokpheng" });
            _authors.Add(new Author() { Id = 29, Name = "rayuth" });
            _authors.Add(new Author() { Id = 30, Name = "thida" });
            _authors.Add(new Author() { Id = 31, Name = "bopha" });
            _authors.Add(new Author() { Id = 32, Name = "sreyna" });
            _authors.Add(new Author() { Id = 33, Name = "sothen" });
            _authors.Add(new Author() { Id = 34, Name = "Chhay Kuntheany" });
            _authors.Add(new Author() { Id = 35, Name = "retchantha" });
            _authors.Add(new Author() { Id = 36, Name = "Yin Piseth" });
            _authors.Add(new Author() { Id = 37, Name = "garoda" });
            _authors.Add(new Author() { Id = 38, Name = "pichvisal" });
            _authors.Add(new Author() { Id = 39, Name = "bunban" });
            _authors.Add(new Author() { Id = 40, Name = "mamsreynoch" });
            _authors.Add(new Author() { Id = 41, Name = "vavattanak" });
            _authors.Add(new Author() { Id = 42, Name = "tivea" });
            _authors.Add(new Author() { Id = 43, Name = "yinpiseth" });
            _authors.Add(new Author() { Id = 44, Name = "leakhena" });
            _authors.Add(new Author() { Id = 45, Name = "vuthy" });
            _authors.Add(new Author() { Id = 46, Name = "sara" });
            _authors.Add(new Author() { Id = 47, Name = "huysokpheng" });
            _authors.Add(new Author() { Id = 48, Name = "sothea" });
            _authors.Add(new Author() { Id = 49, Name = "chumpo" });



        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "KSPGAnalytics-eb982ea20fcc.p12");
            var gaServices = AuthenticateServiceAccount("998452416393-gvh4sfqieh6u9r30hgfbo4j424kv3h7c@developer.gserviceaccount.com", filePath);

            var md = new LinqMetaData();
            StringBuilder sb = new StringBuilder(1024);
            //for (int i = 0; i < 30; i++)
            //{
            var articles = md.Article.Where(t => t.ReporterId > 0 && (t.DateCreated > new DateTime(2015, 2, 26) && t.DateCreated <= new DateTime(2015, 5, 25)));
            foreach (var article in articles)
            {
                try
                {
                    //24392469 koh
                    //92294845 beta


                    //DataResource.GaResource.GetRequest request = gaServices.Data.Ga.Get("ga:24392469", article.DateCreated.ToString("yyyy-MM-dd"), article.DateCreated.AddDays(7).ToString("yyyy-MM-dd"), "ga:pageviews,ga:sessionDuration");
                    DataResource.GaResource.GetRequest request = gaServices.Data.Ga.Get("ga:24392469", article.DateCreated.ToString("yyyy-MM-dd"), article.DateCreated.AddDays(7).ToString("yyyy-MM-dd"), "ga:pageviews,ga:sessionDuration");
                    request.Dimensions = "ga:pagePath";
                    request.Filters = string.Format("ga:pagePath==/article/{0}.html", article.ArticleId);
                    GaData result = request.Execute();
                    foreach (var row in result.Rows)
                    {
                        foreach (string col in row)
                        {
                            _articleViews.Add(new ArticleView() { ArticleId = article.ArticleId, View = int.Parse(row[1]) });
                            sb.Append(col + " ");  // writes the value of the column
                        }
                        sb.Append("\r\n");
                    }
                    System.Threading.Thread.Sleep(1000);

                    //sb.AppendFormat("{0}\t{1}\r\n", article.ArticleId, i + 1);
                }
                catch (Exception)
                {
                }

            }

            //}
            textBox1.Text = sb.ToString();
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Done");


        }

        private void button2_Click(object sender, EventArgs e)
        {
            var md = new LinqMetaData();
            var articles =
                md.Article.Where(
                    t =>
                        t.IsPublished && t.DateCreated.Date == monthCal.SelectionStart.Date &&
                        t.Title.Contains(textBox2.Text));

            //dataGridView1.DataSource = articles.ToList();


            StringBuilder sb = new StringBuilder(1024);

            foreach (var article in articles)
            {

                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\r\n", getArticleTags(article), article.ArticleId, article.Title, article.AuthorId, article.DateCreated, article.DateCreated.Day, article.DateCreated.Month);
            }
            textBox1.Text = sb.ToString();
        }

        private string getArticleTags(ArticleEntity article)
        {
            List<string> tags = new List<string>(50);
            foreach (var tag in article.TagCollectionViaArticleTag)
            {
                tags.Add(tag.Tag);
            }

            if (tags.Count > 0)
                return string.Join(",", tags.ToArray());

            return string.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InitializeReporter();
            InitializeArticleViews();
            InitializeAuthors();
            var md = new LinqMetaData();
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();

            var file1 = new FileInfo(@"C:\Users\Sothy\Desktop\Reporter Report\All.xlsx");
            using (ExcelPackage package1 = new ExcelPackage(file1))
            {
                var ws1 = package1.Workbook.Worksheets.Add("Report");
                ws1.Cells.Style.Font.Size = 9; //Default font size for whole sheet
                ws1.Cells.Style.Font.Name = "Khmer OS Content";

                ws1.Cells[1, 1].Value = "ថ្ងៃ";
                ws1.Cells[1, 2].Value = "ចំណងជើង";
                ws1.Cells[1, 3].Value = "លេខកូដ";
                ws1.Cells[1, 4].Value = "ចំនួនទស្សនា";
                ws1.Cells[1, 5].Value = "ចំណាត់ថ្នាក់";
                ws1.Cells[1, 6].Value = "វីដេអូ";
                ws1.Cells[1, 7].Value = "ទឹក​ប្រាក់";
                ws1.Cells[1, 8].Value = "ReporterId";
                ws1.Cells[1, 9].Value = "ឈ្មោះ";


                ws1.Cells[1, 1, 1, 9].Style.Font.Name = "Khmer OS Content";
                ws1.Cells[1, 1, 1, 9].Style.Font.Size = 10;
                ws1.Cells[1, 1, 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws1.Cells[1, 1, 1, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws1.Column(1).Width = 10;
                ws1.Column(2).Width = 40;
                ws1.Column(3).Width = 8;
                ws1.Column(4).Width = 8;
                ws1.Column(5).Width = 8;
                ws1.Column(6).Width = 8;
                ws1.Column(7).Width = 8;
                ws1.Column(8).Width = 9;
                ws1.Column(9).Width = 12;

                int allRow = 0;


                foreach (var reporter in _reporters.OrderBy(t => t.Order))
                {
                    var articles =
                        md.Article.Where(
                            t =>
                                t.IsPublished && (t.ReporterId > 0 && t.ReporterId == reporter.Id) &&
                                (t.DateCreated.Date > new DateTime(2015, 2, 26) && t.DateCreated.Date <= new DateTime(2015, 5, 25))).OrderBy(t => t.DateCreated);


                    int totalRow = articles.Count() + 1;



                    if (totalRow > 1)
                    {
                        sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\r\n", "Date", "ID", "Title",
                            "Reporter ID", "Reporter Name", "Reporter Provice", "View Count", "Range", "Has Video",
                            "Revenue", "AuthorID", "Author Revenue");

                        sb1.AppendFormat("{0}\t{1}\t{2}\t{3}\r\n", "AuthorID", "ArticleId", "Range", "Revenue(range)");

                        var file = new FileInfo(Path.Combine(@"C:\Users\Sothy\Desktop\Reporter Report", string.Concat(reporter.Name, ".xlsx")));
                        using (ExcelPackage package = new ExcelPackage(file))
                        {
                            var ws = package.Workbook.Worksheets.Add("Report1");
                            ws.Cells.Style.Font.Size = 9; //Default font size for whole sheet
                            ws.Cells.Style.Font.Name = "Khmer OS Content";

                            ws.Cells[1, 1].Value = "ថ្ងៃ";
                            ws.Cells[1, 2].Value = "ចំណងជើង";
                            ws.Cells[1, 3].Value = "លេខកូដ";
                            ws.Cells[1, 4].Value = "ចំនួនទស្សនា";
                            ws.Cells[1, 5].Value = "ចំណាត់ថ្នាក់";
                            ws.Cells[1, 6].Value = "វីដេអូ";
                            ws.Cells[1, 7].Value = "ទឹក​ប្រាក់";

                            ws.Cells[1, 1, 1, 7].Style.Font.Name = "Khmer OS Content";
                            ws.Cells[1, 1, 1, 7].Style.Font.Size = 10;
                            ws.Cells[1, 1, 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[1, 1, 1, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Column(1).Width = 10;
                            ws.Column(2).Width = 40;
                            ws.Column(3).Width = 8;
                            ws.Column(4).Width = 8;
                            ws.Column(5).Width = 8;
                            ws.Column(6).Width = 8;
                            ws.Column(7).Width = 8;



                            var border = ws.Cells[1, 1, totalRow, 7].Style.Border;
                            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                            int index = 0;

                            foreach (var article in articles)
                            {
                                bool isBreakingNews = (article.MinuteLate > 0 && article.MinuteLate <= 15);

                                bool hasVideo = !string.IsNullOrEmpty(article.VideoUrl);

                                int range = 4;

                                int articleView = 0;

                                var tmpArticle = _articleViews.Where(t => t.ArticleId == article.ArticleId);
                                if (tmpArticle.Any())
                                    articleView = tmpArticle.FirstOrDefault().View;

                                if (articleView <= 8000)
                                    articleView = (int)((articleView / 100) * 40) + articleView;

                                if (articleView >= 13000)
                                {
                                    if (isBreakingNews)
                                        range = 1;
                                    else
                                        range = 2;
                                }
                                else if (articleView >= 10000)
                                    range = 2;
                                else if (articleView >= 8000)
                                    range = 3;

                                if (range <= 2)
                                    sb1.AppendFormat("{0}\t{1}\t{2}\t{3}\r\n", article.AuthorId, article.ArticleId, range,
                                        GetAuthorRevenue(range));


                                ws.Cells[index + 2, 1].Value = article.DateCreated.ToString("dd-MM-yyyy");
                                ws.Cells[index + 2, 2].Value = article.Title;
                                ws.Cells[index + 2, 3].Value = article.ArticleId;
                                ws.Cells[index + 2, 4].Value = articleView;
                                ws.Cells[index + 2, 5].Value = range;
                                ws.Cells[index + 2, 6].Value = hasVideo ? "មាន" : "គ្មាន";
                                ws.Cells[index + 2, 7].Value = GetRevenue(range, hasVideo);

                                ws1.Cells[allRow + 2, 1].Value = article.DateCreated.ToString("dd-MM-yyyy");
                                ws1.Cells[allRow + 2, 2].Value = article.Title;
                                ws1.Cells[allRow + 2, 3].Value = article.ArticleId;
                                ws1.Cells[allRow + 2, 4].Value = articleView;
                                ws1.Cells[allRow + 2, 5].Value = range;
                                ws1.Cells[allRow + 2, 6].Value = hasVideo ? "មាន" : "គ្មាន";
                                ws1.Cells[allRow + 2, 7].Value = GetRevenue(range, hasVideo);
                                ws1.Cells[allRow + 2, 8].Value = reporter.Id;
                                ws1.Cells[allRow + 2, 9].Value = reporter.Name;




                                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\r\n", article.DateCreated.ToString("yyyy-MM-dd"),
                                    article.ArticleId, article.Title, reporter.Id, reporter.Name, reporter.Province, articleView, range, hasVideo ? "1" : "0", GetRevenue(range, hasVideo), article.AuthorId, GetAuthorRevenue(range));

                                index++;

                                allRow++;
                            }

                            ws.Cells[totalRow + 1, 5, totalRow + 1, 5].Value = "Total";
                            ws.Cells[totalRow + 1, 5, totalRow + 1, 6].Merge = true;
                            ws.Cells[totalRow + 1, 5, totalRow + 1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[totalRow + 1, 5, totalRow + 1, 6].Style.Font.Bold = true;
                            ws.Cells[totalRow + 1, 7, totalRow + 1, 7].Formula = "Sum(" + ws.Cells[2, 7].Address + ":" + ws.Cells[totalRow, 7].Address + ")";
                            ws.Cells[totalRow + 1, 7, totalRow + 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[totalRow + 1, 7, totalRow + 1, 7].Style.Font.Bold = true;

                            var border1 = ws.Cells[totalRow + 1, 5, totalRow + 1, 7].Style.Border;
                            border1.Bottom.Style = border1.Top.Style = border1.Left.Style = border1.Right.Style = ExcelBorderStyle.Thin;

                            package.Save();

                        }

                        sb.Append("\r\n\r\n");
                    }



                }

                package1.Save();
            }


            textBox1.Text = sb.ToString();
            textBox3.Text = sb1.ToString();

        }

        private double GetAuthorRevenue(int range)
        {
            if (range == 1)
                return 0.8;
            else if (range == 2)
                return 0.5;
            else return 0;
        }

        private double GetRevenue(int range, bool hasVideo)
        {
            double returnValue = 0;
            switch (range)
            {
                case 1:
                    returnValue = 3;
                    break;
                case 2:
                    returnValue = 1.5;
                    break;
                case 3:
                    returnValue = 1;
                    break;
            }
            if (hasVideo)
                returnValue = returnValue + 2;

            return returnValue;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            InitializeReporter();
            InitializeArticleViews();
            InitializeAuthors();

            var md = new LinqMetaData();
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();

            foreach (System.IO.FileInfo file in new DirectoryInfo(@"C:\Users\Sothy\Desktop\Reporter Report").GetFiles()) file.Delete();

            var file1 = new FileInfo(@"C:\Users\Sothy\Desktop\Reporter Report\All.xlsx");
            using (ExcelPackage package1 = new ExcelPackage(file1))
            {
                var ws1 = package1.Workbook.Worksheets.Add("Report");
                ws1.Cells.Style.Font.Size = 9; //Default font size for whole sheet
                ws1.Cells.Style.Font.Name = "Khmer OS Content";

                ws1.Cells[1, 1].Value = "ថ្ងៃ";
                ws1.Cells[1, 2].Value = "ចំណងជើង";
                ws1.Cells[1, 3].Value = "លេខកូដ";
                ws1.Cells[1, 4].Value = "ចំនួនទស្សនា";
                ws1.Cells[1, 5].Value = "ចំណាត់ថ្នាក់";
                ws1.Cells[1, 6].Value = "វីដេអូ";
                ws1.Cells[1, 7].Value = "ទឹក​ប្រាក់";
                ws1.Cells[1, 8].Value = "ReporterId";
                ws1.Cells[1, 9].Value = "ឈ្មោះ";


                ws1.Cells[1, 1, 1, 9].Style.Font.Name = "Khmer OS Content";
                ws1.Cells[1, 1, 1, 9].Style.Font.Size = 10;
                ws1.Cells[1, 1, 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws1.Cells[1, 1, 1, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws1.Column(1).Width = 10;
                ws1.Column(2).Width = 40;
                ws1.Column(3).Width = 8;
                ws1.Column(4).Width = 8;
                ws1.Column(5).Width = 8;
                ws1.Column(6).Width = 8;
                ws1.Column(7).Width = 8;
                ws1.Column(8).Width = 9;
                ws1.Column(9).Width = 12;

                int allRow = 0;

                var authors = new List<Author>();

                foreach (var reporter in _reporters.OrderBy(t => t.Order))
                {
                    var articles =
                        md.Article.Where(
                            t =>
                                t.IsPublished && (t.ReporterId > 0 && t.ReporterId == reporter.Id) &&
                                (t.DateCreated.Date > new DateTime(2015, 2, 26) && t.DateCreated.Date <= new DateTime(2015, 5, 25))).OrderBy(t => t.DateCreated);


                    int totalRow = articles.Count() + 1;



                    if (totalRow > 1)
                    {
                        sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\r\n", "Date", "ID", "Title",
                            "Reporter ID", "Reporter Name", "Reporter Provice", "View Count", "Range", "Has Video",
                            "Revenue", "AuthorID", "Author Revenue");

                        sb1.AppendFormat("{0}\t{1}\t{2}\t{3}\r\n", "AuthorID", "ArticleId", "Range", "Revenue(range)");

                        var file = new FileInfo(Path.Combine(@"C:\Users\Sothy\Desktop\Reporter Report", string.Concat(reporter.Name, ".xlsx")));

                        int A = 0, B = 0, C = 0, D = 0, V = 0, T = 0;

                        using (ExcelPackage package = new ExcelPackage(file))
                        {
                            var ws = package.Workbook.Worksheets.Add("Report");
                            ws.Cells.Style.Font.Size = 9; //Default font size for whole sheet
                            ws.Cells.Style.Font.Name = "Khmer OS Content";

                            ws.Cells[1, 1].Value = "ថ្ងៃ";
                            ws.Cells[1, 2].Value = "ចំណងជើង";
                            ws.Cells[1, 3].Value = "លេខកូដ";
                            ws.Cells[1, 4].Value = "ចំនួនទស្សនា";
                            ws.Cells[1, 5].Value = "ចំណាត់ថ្នាក់";
                            ws.Cells[1, 6].Value = "វីដេអូ";
                            ws.Cells[1, 7].Value = "ទឹក​ប្រាក់";

                            ws.Cells[1, 1, 1, 7].Style.Font.Name = "Khmer OS Content";
                            ws.Cells[1, 1, 1, 7].Style.Font.Size = 10;
                            ws.Cells[1, 1, 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[1, 1, 1, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ws.Column(1).Width = 10;
                            ws.Column(2).Width = 40;
                            ws.Column(3).Width = 8;
                            ws.Column(4).Width = 8;
                            ws.Column(5).Width = 8;
                            ws.Column(6).Width = 8;
                            ws.Column(7).Width = 8;



                            var border = ws.Cells[1, 1, totalRow, 7].Style.Border;
                            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                            int index = 0;

                            foreach (var article in articles)
                            {
                                bool isBreakingNews = (article.MinuteLate > 0 && article.MinuteLate <= 15);

                                bool hasVideo = !string.IsNullOrEmpty(article.VideoUrl);

                                int range = 4;

                                int articleView = 0;

                                var tmpArticle = _articleViews.Where(t => t.ArticleId == article.ArticleId);
                                if (tmpArticle.Any())
                                    articleView = tmpArticle.FirstOrDefault().View;

                                if (articleView <= 8000)
                                    articleView = (int)((articleView / 100) * 40) + articleView;

                                if (articleView >= 13000)
                                {
                                    if (isBreakingNews)
                                        range = 1;
                                    else
                                        range = 2;
                                }
                                else if (articleView >= 10000)
                                    range = 2;
                                else if (articleView >= 8000)
                                    range = 3;

                                if (range == 1)
                                    A++;
                                else if (range == 2)
                                    B++;
                                else if (range == 3)
                                    C++;
                                else
                                    D++;

                                T++;

                                if (hasVideo)
                                    V++;

                                var tmp = _authors.Where(t => t.Id == article.AuthorId).Single();
                                if (range <= 2)
                                {
                                    sb1.AppendFormat("{0}\t{1}\t{2}\t{3}\r\n", article.AuthorId, article.ArticleId, range,
                                        GetAuthorRevenue(range));


                                    if (range == 1)
                                    {
                                        tmp.TotalA = tmp.TotalA + 1;
                                    }
                                    else
                                    {
                                        tmp.TotalB = tmp.TotalB + 1;
                                    }                                    
                                }
                                
                                tmp.TotalArticles = tmp.TotalArticles + 1;

                                ws.Cells[index + 2, 1].Value = article.DateCreated.ToString("dd-MM-yyyy");
                                ws.Cells[index + 2, 2].Value = article.Title;
                                ws.Cells[index + 2, 3].Value = article.ArticleId;
                                ws.Cells[index + 2, 4].Value = articleView;
                                ws.Cells[index + 2, 5].Value = range;
                                ws.Cells[index + 2, 6].Value = hasVideo ? "មាន" : "គ្មាន";
                                ws.Cells[index + 2, 7].Value = GetRevenue(range, hasVideo);

                                ws1.Cells[allRow + 2, 1].Value = article.DateCreated.ToString("dd-MM-yyyy");
                                ws1.Cells[allRow + 2, 2].Value = article.Title;
                                ws1.Cells[allRow + 2, 3].Value = article.ArticleId;
                                ws1.Cells[allRow + 2, 4].Value = articleView;
                                ws1.Cells[allRow + 2, 5].Value = range;
                                ws1.Cells[allRow + 2, 6].Value = hasVideo ? "មាន" : "គ្មាន";
                                ws1.Cells[allRow + 2, 7].Value = GetRevenue(range, hasVideo);
                                ws1.Cells[allRow + 2, 8].Value = reporter.Id;
                                ws1.Cells[allRow + 2, 9].Value = reporter.Name;




                                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\r\n", article.DateCreated.ToString("yyyy-MM-dd"),
                                    article.ArticleId, article.Title, reporter.Id, reporter.Name, reporter.Province, articleView, range, hasVideo ? "1" : "0", GetRevenue(range, hasVideo), article.AuthorId, GetAuthorRevenue(range));

                                index++;

                                allRow++;
                            }
                            reporter.TotalA = A;
                            reporter.TotalB = B;
                            reporter.TotalC = C;
                            reporter.TotalD = D;
                            reporter.TotalV = V;
                            reporter.TotalArticles = T;


                            ws.Cells[totalRow + 1, 5, totalRow + 1, 5].Value = "Total";
                            ws.Cells[totalRow + 1, 5, totalRow + 1, 6].Merge = true;
                            ws.Cells[totalRow + 1, 5, totalRow + 1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[totalRow + 1, 5, totalRow + 1, 6].Style.Font.Bold = true;
                            ws.Cells[totalRow + 1, 7, totalRow + 1, 7].Formula = "Sum(" + ws.Cells[2, 7].Address + ":" + ws.Cells[totalRow, 7].Address + ")";
                            ws.Cells[totalRow + 1, 7, totalRow + 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            ws.Cells[totalRow + 1, 7, totalRow + 1, 7].Style.Font.Bold = true;

                            var border1 = ws.Cells[totalRow + 1, 5, totalRow + 1, 7].Style.Border;
                            border1.Bottom.Style = border1.Top.Style = border1.Left.Style = border1.Right.Style = ExcelBorderStyle.Thin;

                            package.Save();

                        }

                        sb.Append("\r\n\r\n");
                    }



                }

                package1.Save();
            }

            var file2 = new FileInfo(@"C:\Users\Sothy\Desktop\Reporter Report\AllReporters.xlsx");
            using (ExcelPackage package2 = new ExcelPackage(file2))
            {
                var ws2 = package2.Workbook.Worksheets.Add("Phnom Penh");
                ws2.Cells.Style.Font.Size = 9; //Default font size for whole sheet
                ws2.Cells.Style.Font.Name = "Khmer OS Content";

                ws2.Cells[1, 1].Value = "Nº";
                ws2.Cells[1, 2].Value = "ឈ្មោះ";
                ws2.Cells[1, 3].Value = "សរុបអត្ថបទ";
                ws2.Cells[1, 4].Value = "ចំណាត់ថ្នាក់";
                ws2.Cells[1, 4, 1, 5].Merge = true;
                ws2.Cells[1, 6].Value = "តម្លៃអត្តបទ";
                ws2.Cells[1, 7].Value = "ប្រាក់ដែលទទួលបាន";
                ws2.Cells[1, 8].Value = "សរុប";


                ws2.Cells[1, 1, 1, 7].Style.Font.Name = "Khmer OS Content";
                ws2.Cells[1, 1, 1, 7].Style.Font.Size = 10;
                ws2.Cells[1, 1, 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws2.Cells[1, 1, 1, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws2.Column(1).Width = 10;
                ws2.Column(2).Width = 40;
                ws2.Column(3).Width = 8;
                ws2.Column(4).Width = 8;
                ws2.Column(5).Width = 8;
                ws2.Column(6).Width = 8;
                ws2.Column(7).Width = 8;



                int rowNumber = 0, reporterNumber = 0;
                foreach (var reporter in _reporters.Where(t=>t.Province.ToLower()=="phnom penh"))
                {
                    ws2.Cells[(reporterNumber * 5) + 2, 1].Value = reporterNumber + 1;
                    ws2.Cells[(reporterNumber * 5) + 2, 1, (reporterNumber * 5) + 6, 1].Merge = true;
                    ws2.Cells[(reporterNumber * 5) + 2, 1, (reporterNumber * 5) + 6, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws2.Cells[(reporterNumber * 5) + 2, 1, (reporterNumber * 5) + 6, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;                                                            

                    ws2.Cells[(reporterNumber * 5) + 2, 2].Value = reporter.Name;
                    ws2.Cells[(reporterNumber * 5) + 2, 2, (reporterNumber * 5) + 6, 2].Merge = true;
                    ws2.Cells[(reporterNumber * 5) + 2, 2, (reporterNumber * 5) + 6, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                    ws2.Cells[(reporterNumber * 5) + 2, 2, (reporterNumber * 5) + 6, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    

                    ws2.Cells[(reporterNumber * 5) + 2, 3].Value = reporter.TotalArticles;
                    ws2.Cells[(reporterNumber * 5) + 2, 3, (reporterNumber * 5) + 6, 3].Merge = true;
                    ws2.Cells[(reporterNumber * 5) + 2, 3, (reporterNumber * 5) + 6, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                    ws2.Cells[(reporterNumber * 5) + 2, 3, (reporterNumber * 5) + 6, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    

                    ws2.Cells[(reporterNumber * 5) + 2, 4].Value = "A:";
                    ws2.Cells[(reporterNumber * 5) + 2, 5].Value = reporter.TotalA;
                    ws2.Cells[(reporterNumber * 5) + 2, 6].Value = 2;
                    ws2.Cells[(reporterNumber * 5) + 2, 7].Value = 2 * reporter.TotalA;

                    ws2.Cells[(reporterNumber * 5) + 3, 4].Value = "B:";
                    ws2.Cells[(reporterNumber * 5) + 3, 5].Value = reporter.TotalB;
                    ws2.Cells[(reporterNumber * 5) + 3, 6].Value = 1.5;
                    ws2.Cells[(reporterNumber * 5) + 3, 7].Value = 1.5 * reporter.TotalB;

                    ws2.Cells[(reporterNumber * 5) + 4, 4].Value = "C:";
                    ws2.Cells[(reporterNumber * 5) + 4, 5].Value = reporter.TotalC;
                    ws2.Cells[(reporterNumber * 5) + 4, 6].Value = 1;
                    ws2.Cells[(reporterNumber * 5) + 4, 7].Value = 1 * reporter.TotalC;

                    ws2.Cells[(reporterNumber * 5) + 5, 4].Value = "D:";
                    ws2.Cells[(reporterNumber * 5) + 5, 5].Value = reporter.TotalD;
                    ws2.Cells[(reporterNumber * 5) + 5, 6].Value = 0;
                    ws2.Cells[(reporterNumber * 5) + 5, 7].Value = 0;

                    ws2.Cells[(reporterNumber * 5) + 6, 4].Value = "Video:";
                    ws2.Cells[(reporterNumber * 5) + 6, 5].Value = reporter.TotalV;
                    ws2.Cells[(reporterNumber * 5) + 6, 6].Value = 2;
                    ws2.Cells[(reporterNumber * 5) + 6, 7].Value = 2 * reporter.TotalV;

                    ws2.Cells[(reporterNumber * 5) + 2, 8].Value = (2 * reporter.TotalA) + (1.5 * reporter.TotalB) + (1 * reporter.TotalC) + (2 * reporter.TotalV);
                    ws2.Cells[(reporterNumber * 5) + 2, 8, (reporterNumber * 5) + 6, 8].Merge = true;
                    ws2.Cells[(reporterNumber * 5) + 2, 8, (reporterNumber * 5) + 6, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws2.Cells[(reporterNumber * 5) + 2, 8, (reporterNumber * 5) + 6, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var border = ws2.Cells[(reporterNumber * 5) + 2, 1, (reporterNumber * 5) + 6, 8].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[(reporterNumber * 5) + 2, 1, (reporterNumber * 5) + 6, 8].Style.Border = border;

                    reporterNumber++;
                }


                var ws3 = package2.Workbook.Worksheets.Add("Province");
                ws3.Cells.Style.Font.Size = 9; //Default font size for whole sheet
                ws3.Cells.Style.Font.Name = "Khmer OS Content";

                ws3.Cells[1, 1].Value = "Nº";
                ws3.Cells[1, 2].Value = "ឈ្មោះ";
                ws3.Cells[1, 3].Value = "សរុបអត្ថបទ";
                ws3.Cells[1, 4].Value = "ចំណាត់ថ្នាក់";
                ws3.Cells[1, 4, 1, 5].Merge = true;
                ws3.Cells[1, 6].Value = "តម្លៃអត្តបទ";
                ws3.Cells[1, 7].Value = "ប្រាក់ដែលទទួលបាន";
                ws3.Cells[1, 8].Value = "សរុប";


                ws3.Cells[1, 1, 1, 7].Style.Font.Name = "Khmer OS Content";
                ws3.Cells[1, 1, 1, 7].Style.Font.Size = 10;
                ws3.Cells[1, 1, 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws3.Cells[1, 1, 1, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws3.Column(1).Width = 10;
                ws3.Column(2).Width = 40;
                ws3.Column(3).Width = 8;
                ws3.Column(4).Width = 8;
                ws3.Column(5).Width = 8;
                ws3.Column(6).Width = 8;
                ws3.Column(7).Width = 8;



                rowNumber = 0;
                reporterNumber = 0;
                foreach (var reporter in _reporters.Where(t=>!t.Province.Equals("phnom penh",StringComparison.InvariantCultureIgnoreCase)))
                {                    

                    ws3.Cells[(reporterNumber * 5) + 2, 1].Value = reporterNumber + 1;
                    ws3.Cells[(reporterNumber * 5) + 2, 1, (reporterNumber * 5) + 6, 1].Merge = true;
                    ws3.Cells[(reporterNumber * 5) + 2, 1, (reporterNumber * 5) + 6, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws3.Cells[(reporterNumber * 5) + 2, 1, (reporterNumber * 5) + 6, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws3.Cells[(reporterNumber * 5) + 2, 2].Value = reporter.Name;
                    ws3.Cells[(reporterNumber * 5) + 2, 2, (reporterNumber * 5) + 6, 2].Merge = true;
                    ws3.Cells[(reporterNumber * 5) + 2, 2, (reporterNumber * 5) + 6, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws3.Cells[(reporterNumber * 5) + 2, 2, (reporterNumber * 5) + 6, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    ws3.Cells[(reporterNumber * 5) + 2, 3].Value = reporter.TotalArticles;
                    ws3.Cells[(reporterNumber * 5) + 2, 3, (reporterNumber * 5) + 6, 3].Merge = true;
                    ws3.Cells[(reporterNumber * 5) + 2, 3, (reporterNumber * 5) + 6, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws3.Cells[(reporterNumber * 5) + 2, 3, (reporterNumber * 5) + 6, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    ws3.Cells[(reporterNumber * 5) + 2, 4].Value = "A:";
                    ws3.Cells[(reporterNumber * 5) + 2, 5].Value = reporter.TotalA;
                    ws3.Cells[(reporterNumber * 5) + 2, 6].Value = 2;
                    ws3.Cells[(reporterNumber * 5) + 2, 7].Value = 2 * reporter.TotalA;

                    ws3.Cells[(reporterNumber * 5) + 3, 4].Value = "B:";
                    ws3.Cells[(reporterNumber * 5) + 3, 5].Value = reporter.TotalB;
                    ws3.Cells[(reporterNumber * 5) + 3, 6].Value = 1.5;
                    ws3.Cells[(reporterNumber * 5) + 3, 7].Value = 1.5 * reporter.TotalB;

                    ws3.Cells[(reporterNumber * 5) + 4, 4].Value = "C:";
                    ws3.Cells[(reporterNumber * 5) + 4, 5].Value = reporter.TotalC;
                    ws3.Cells[(reporterNumber * 5) + 4, 6].Value = 1;
                    ws3.Cells[(reporterNumber * 5) + 4, 7].Value = 1 * reporter.TotalC;

                    ws3.Cells[(reporterNumber * 5) + 5, 4].Value = "D:";
                    ws3.Cells[(reporterNumber * 5) + 5, 5].Value = reporter.TotalD;
                    ws3.Cells[(reporterNumber * 5) + 5, 6].Value = 0;
                    ws3.Cells[(reporterNumber * 5) + 5, 7].Value = 0;

                    ws3.Cells[(reporterNumber * 5) + 6, 4].Value = "Video:";
                    ws3.Cells[(reporterNumber * 5) + 6, 5].Value = reporter.TotalV;
                    ws3.Cells[(reporterNumber * 5) + 6, 6].Value = 2;
                    ws3.Cells[(reporterNumber * 5) + 6, 7].Value = 2 * reporter.TotalV;

                    ws3.Cells[(reporterNumber * 5) + 2, 8].Value = (2 * reporter.TotalA) + (1.5 * reporter.TotalB) + (1 * reporter.TotalC) + (2 * reporter.TotalV);
                    ws3.Cells[(reporterNumber * 5) + 2, 8, (reporterNumber * 5) + 6, 8].Merge = true;
                    ws3.Cells[(reporterNumber * 5) + 2, 8, (reporterNumber * 5) + 6, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws3.Cells[(reporterNumber * 5) + 2, 8, (reporterNumber * 5) + 6, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var border = ws3.Cells[(reporterNumber * 5) + 2, 1, (reporterNumber * 5) + 6, 8].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    ws3.Cells[(reporterNumber * 5) + 2, 1, (reporterNumber * 5) + 6, 8].Style.Border = border;

                    reporterNumber++;
                }

                package2.Save();
            }

            var file3 = new FileInfo(@"C:\Users\Sothy\Desktop\Reporter Report\AllAuthors.xlsx");
            using (ExcelPackage package3 = new ExcelPackage(file3))
            {
                var ws3 = package3.Workbook.Worksheets.Add("Report");
                ws3.Cells.Style.Font.Size = 9; //Default font size for whole sheet
                ws3.Cells.Style.Font.Name = "Khmer OS Content";

                ws3.Cells[1, 1].Value = "Nº";
                ws3.Cells[1, 2].Value = "ឈ្មោះ";
                ws3.Cells[1, 3].Value = "សរុបអត្ថបទ";
                ws3.Cells[1, 4].Value = "ចំណាត់ថ្នាក់";
                ws3.Cells[1, 4, 1, 5].Merge = true;
                ws3.Cells[1, 6].Value = "តម្លៃអត្តបទ";
                ws3.Cells[1, 7].Value = "ប្រាក់ដែលទទួលបាន";
                ws3.Cells[1, 8].Value = "សរុប";


                ws3.Cells[1, 1, 1, 7].Style.Font.Name = "Khmer OS Content";
                ws3.Cells[1, 1, 1, 7].Style.Font.Size = 10;
                ws3.Cells[1, 1, 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws3.Cells[1, 1, 1, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws3.Column(1).Width = 10;
                ws3.Column(2).Width = 40;
                ws3.Column(3).Width = 8;
                ws3.Column(4).Width = 8;
                ws3.Column(5).Width = 8;
                ws3.Column(6).Width = 8;
                ws3.Column(7).Width = 8;



                int reporterNumber = 0;
                foreach (var reporter in _authors.Where(t=>t.TotalArticles>0))
                {
                    ////(0*4)+2
                    //ws3.Cells[2, 1].Value = 1;
                    //ws3.Cells[2, 1, 5, 1].Merge = true;

                    ////(1*4)+2
                    //ws3.Cells[6, 1].Value = 2;
                    //ws3.Cells[6, 1, 9, 1].Merge = true;

                    ////(2*4)+2
                    //ws3.Cells[10, 1].Value = 3;
                    //ws3.Cells[10, 1, 13, 1].Merge = true;

                    ////(3*4)+2
                    //ws3.Cells[14, 1].Value = 3;
                    //ws3.Cells[14, 1, 13, 1].Merge = true;



                    ws3.Cells[(reporterNumber * 3) + 2, 1].Value = reporterNumber + 1;
                    ws3.Cells[(reporterNumber * 3) + 2, 1, (reporterNumber * 3) + 4, 1].Merge = true;
                    ws3.Cells[(reporterNumber * 3) + 2, 1, (reporterNumber * 3) + 4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws3.Cells[(reporterNumber * 3) + 2, 1, (reporterNumber * 3) + 4, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws3.Cells[(reporterNumber * 3) + 2, 2].Value = reporter.Name;
                    ws3.Cells[(reporterNumber * 3) + 2, 2, (reporterNumber * 3) + 4, 2].Merge = true;
                    ws3.Cells[(reporterNumber * 3) + 2, 2, (reporterNumber * 3) + 4, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws3.Cells[(reporterNumber * 3) + 2, 2, (reporterNumber * 3) + 4, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    ws3.Cells[(reporterNumber * 3) + 2, 3].Value = reporter.TotalArticles;
                    ws3.Cells[(reporterNumber * 3) + 2, 3, (reporterNumber * 3) + 4, 3].Merge = true;
                    ws3.Cells[(reporterNumber * 3) + 2, 3, (reporterNumber * 3) + 4, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws3.Cells[(reporterNumber * 3) + 2, 3, (reporterNumber * 3) + 4, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    ws3.Cells[(reporterNumber * 3) + 2, 4].Value = "A:";
                    ws3.Cells[(reporterNumber * 3) + 2, 5].Value = reporter.TotalA;
                    ws3.Cells[(reporterNumber * 3) + 2, 6].Value = 0.8;
                    ws3.Cells[(reporterNumber * 3) + 2, 7].Value = 0.8 * reporter.TotalA;

                    ws3.Cells[(reporterNumber * 3) + 3, 4].Value = "B:";
                    ws3.Cells[(reporterNumber * 3) + 3, 5].Value = reporter.TotalB;
                    ws3.Cells[(reporterNumber * 3) + 3, 6].Value = 0.5;
                    ws3.Cells[(reporterNumber * 3) + 3, 7].Value = 0.5 * reporter.TotalB;

                    ws3.Cells[(reporterNumber * 3) + 4, 4].Value = "C & D:";
                    ws3.Cells[(reporterNumber * 3) + 4, 5].Value = reporter.TotalArticles - (reporter.TotalA + reporter.TotalB);
                    ws3.Cells[(reporterNumber * 3) + 4, 6].Value = 0;
                    ws3.Cells[(reporterNumber * 3) + 4, 7].Value = 0;                    

                    ws3.Cells[(reporterNumber * 3) + 2, 8].Value = (0.8 * reporter.TotalA) + (0.5 * reporter.TotalB) ;
                    ws3.Cells[(reporterNumber * 3) + 2, 8, (reporterNumber * 3) + 4, 8].Merge = true;
                    ws3.Cells[(reporterNumber * 3) + 2, 8, (reporterNumber * 3) + 4, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws3.Cells[(reporterNumber * 3) + 2, 8, (reporterNumber * 3) + 4, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var border = ws3.Cells[(reporterNumber * 3) + 2, 1, (reporterNumber * 3) + 4, 8].Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    ws3.Cells[(reporterNumber * 3) + 2, 1, (reporterNumber * 3) + 4, 8].Style.Border = border;

                    reporterNumber++;
                }

                package3.Save();
            }



            textBox1.Text = sb.ToString();
            textBox3.Text = sb1.ToString();
        }
    }
}
