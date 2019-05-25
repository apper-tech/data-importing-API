using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml.Linq;

namespace DataImporting
{
   

    public class StaticCalls
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DB_9FEE73_RealEstateDBConnectionString"].ConnectionString;

        public static List<City> citylist = new List<City>();

        public static List<string> propertyidlist = new List<string>();

        public static List<string> imagenames = new List<string>();

        private static string MailServer
        {
            get
            {
                return WebConfigurationManager.AppSettings["Host_MailServer"];
            }
        }

        private static string InfoUserName
        {
            get
            {
                return WebConfigurationManager.AppSettings["Host_Mail_Info_Username"];
            }
        }

        private static string InfoPassword
        {
            get
            {
                return WebConfigurationManager.AppSettings["Host_Mail_Info_Password"];
            }
        }

        public static void SendLogMail(string txtTo, string subj, string body)
        {
            StaticCalls.SendMail(StaticCalls.InfoUserName, StaticCalls.InfoPassword, StaticCalls.MailServer, subj, body, txtTo);
        }

        private static void SendMail(string username, string password, string host, string title, string body, string recive)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            mailMessage.From = new MailAddress(username);
            mailMessage.To.Add(recive);
            mailMessage.Subject = title;
            MailMessage mailMessage2 = mailMessage;
            mailMessage2.Body += body;
            mailMessage.IsBodyHtml = true;
            smtpClient.Host = host;
            string value = "gmail.com";
            if (username.ToLower().Contains(value))
            {
                try
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(username, password);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    smtpClient.Port = 8889;
                    smtpClient.Credentials = new NetworkCredential(username, password);
                    smtpClient.EnableSsl = false;
                    smtpClient.Send(mailMessage);
                }
                catch (Exception)
                {
                }
            }
        }

        public static string URLToString(string url)
        {
            HttpWebResponse httpWebResponse = null;
            Stream stream = null;
            try
            {
                HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(url);
                obj.UserAgent = "Foo";
                obj.Accept = "*/*";
                httpWebResponse = (HttpWebResponse)obj.GetResponse();
                stream = httpWebResponse.GetResponseStream();
                string result = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
                if (stream != null)
                {
                    stream.Close();
                }
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static void InitVars()
        {
            StaticCalls.InitCity();
            StaticCalls.InitNames();
            StaticCalls.InitPropertyIDList();
        }

        public static void send_text(string text)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
            File.AppendAllText(path, text + "\n");
            if (text == "\\c")
            {
                File.WriteAllText(path, "");
            }
        }

        private static void SetQuery(string sql)
        {
            SqlConnection sqlConnection = new SqlConnection(StaticCalls.ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception)
            {
            }
        }

        public static DataTable GetQuery(string sql, string connName, object args)
        {
            if (connName.Contains("Connection"))
            {
                connName = WebConfigurationManager.ConnectionStrings[connName].ConnectionString;
            }
            SqlConnection sqlConnection = new SqlConnection(connName);
            SqlCommand selectCommand = new SqlCommand(sql, sqlConnection);
            try
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                DataTable dataTable = dataSet.Tables[0];
                if (args != null && args.GetType() == typeof(int))
                {
                    for (int i = (int)args - dataTable.Rows.Count; i < 0; i++)
                    {
                        dataTable.Rows.Remove(dataTable.Rows[dataTable.Rows.Count - 1]);
                    }
                    return dataTable;
                }
                return dataTable;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void InsertProperties(List<Property> p)
        {
            dataDataContext dataDataContext = new dataDataContext(StaticCalls.ConnectionString);
            foreach (Property item in p)
            {
                dataDataContext.Properties.InsertOnSubmit(item);
            }
            dataDataContext.SubmitChanges();
        }

        public static void DeleteProperties()
        {
            dataDataContext dataDataContext = new dataDataContext(StaticCalls.ConnectionString);
            List<Property> ls = (from w in dataDataContext.Properties
                                 where w.Property_Id_ext != (string)null
                                 select w).ToList();
            dataDataContext.ExecuteCommand("DELETE FROM [dbo].[Properties] WHERE [Property_Id_ext] IS NOT NULL");
            StaticCalls.send_text("\\c");
            StaticCalls.send_text("Properties Deleted");
            StaticCalls.Remove_Images(ls);
            StaticCalls.send_text("Images Deleted");
        }

        private static void Remove_Images(List<Property> ls)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(new DirectoryInfo((HttpContext.Current != null) ? HttpContext.Current.Request.PhysicalApplicationPath : AppDomain.CurrentDomain.BaseDirectory).Parent.FullName + "\\RealEstate\\PropertyImage");
            foreach (Property l in ls)
            {
                List<string> fileNames = StaticCalls.GetFileNames(l.Property_Photo);
                File.Delete((directoryInfo.FullName + fileNames[0] != null) ? fileNames[0] : "");
                StaticCalls.send_text("Deleted: " + directoryInfo.FullName + fileNames[0]);
                File.Delete((directoryInfo.FullName + fileNames[1] != null) ? fileNames[1] : "");
                StaticCalls.send_text("Deleted: " + directoryInfo.FullName + fileNames[1]);
            }
        }

        private static List<string> GetFileNames(string Path)
        {
            if (!string.IsNullOrEmpty(Path))
            {
                string text = Path;
                Path = text.Remove(text.Length - 1, 1);
                string text2 = "";
                List<string> list = new List<string>();
                for (int i = 0; i < Path.Length; i++)
                {
                    if (Path[i] == '|')
                    {
                        list.Add(text2);
                        Path = Path.Substring(i + 1);
                        i = 0;
                        text2 = "";
                    }
                    text2 += Path[i].ToString();
                }
                list.Add(text2);
                return list;
            }
            return null;
        }

        public static void UpdateProperty(Property v)
        {
            foreach (Property item in new dataDataContext(StaticCalls.ConnectionString).Properties.ToList())
            {
                if (item != null && item.Property_Id_ext == v.Property_Id_ext)
                {
                    StaticCalls.UpdateObj(v, item);
                }
            }
        }

        private static void UpdateObj(Property _old, Property _new)
        {
            _old.Property_Size = ((_new.Property_Size > 0) ? _new.Property_Size : _old.Property_Size);
            _old.Floor = _new.Floor;
            _old.Has_Garden = _new.Has_Garden;
            _old.Has_Garage = _new.Has_Garage;
            _old.Num_Bedrooms = _new.Num_Bedrooms;
            _old.Num_Bathrooms = _new.Num_Bathrooms;
            _old.Expire_Date = DateTime.UtcNow.AddMonths(3);
            _old.Contract_Type = 1;
            _old.Address = _new.Address;
            _old.Location = _new.Location;
            _old.Zip_Code = "";
            _old.Other_Details = _new.Other_Details;
            _old.Sale_Price = _new.Sale_Price;
            _old.Rent_Price = _new.Rent_Price;
            _old.Num_Floors = null;
        }

        public static List<City> GetCityList(string code)
        {
            List<City> list = new List<City>();
            foreach (City item in StaticCalls.citylist)
            {
                if (item.City_ID.ToString() == code)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        private static void InitCity()
        {
            if (StaticCalls.citylist.Count == 0)
            {
                DataTable query = StaticCalls.GetQuery("SELECT City_ID, City_Name, Country_ID, City_Native_Name, Latitude, Longitude FROM Cities", StaticCalls.ConnectionString, null);
                List<City> list = new List<City>();
                try
                {
                    foreach (DataRow row in query.Rows)
                    {
                        list.Add(new City
                        {
                            Country_ID = (int)row[2],
                            City_ID = (int)row[0],
                            City_Name = (string)row[1],
                            City_Native_Name = (string)row[3],
                            Latitude = (string)row[4],
                            Longitude = (string)row[5]
                        });
                    }
                }
                catch
                {
                }
                StaticCalls.citylist = list;
            }
        }

        public static List<string> GetPropertyIDList()
        {
            return StaticCalls.propertyidlist;
        }

        private static void InitPropertyIDList()
        {
            if (StaticCalls.propertyidlist.Count == 0)
            {
                DataTable query = StaticCalls.GetQuery("SELECT Properties.Property_Id_ext from Properties;", StaticCalls.ConnectionString, null);
                List<string> list = new List<string>();
                try
                {
                    foreach (DataRow row in query.Rows)
                    {
                        list.Add(string.Concat(row["Property_Id_ext"]));
                    }
                }
                catch
                {
                }
                StaticCalls.propertyidlist = list;
            }
        }
        private static List<string> Get_Property_List()
        {
            if (StaticCalls.propertyidlist.Count == 0)
            {
                DataTable query = StaticCalls.GetQuery("SELECT Properties.Property_Photo from Properties;", StaticCalls.ConnectionString, null);
                List<string> list = new List<string>();
                try
                {
                    foreach (DataRow row in query.Rows)
                    {
                        list.Add(string.Concat(row["Property_Photo"]));
                    }
                    return list;
                }
                catch
                {
                    return new List<string>();
                }
            }
            return new List<string>();
        }

        public static string InsertCity(City c)
        {
            StaticCalls.SetQuery("Insert Into Cities (City_Name,City_Native_Name,City_Latin_Name,Country_ID,Latitude,Longitude) values ('" + c.City_Name + "','" + c.City_Native_Name + "','" + c.City_Latin_Name + "'," + c.Country_ID + ",'" + c.Latitude + "','" + c.Longitude + "');");
            DataTable query = StaticCalls.GetQuery("select IDENT_CURRENT('Cities') as 'id'", StaticCalls.ConnectionString, null);
            if (query == null)
            {
                return "1";
            }
            return query.Rows[0][0].ToString();
        }

        public static City GetCityData(string reigon, string address, string cid)
        {
            XElement xElement = XElement.Load("http://maps.googleapis.com/maps/api/geocode/xml?address=" + address + "&reigon=" + reigon);
            if (xElement.Element("status").Value == "OK")
            {
                return new City
                {
                    City_Name = string.Format("{0}", xElement.Element("result").Element("address_component").Element("long_name")
                        .Value),
                    City_Native_Name = string.Format("{0}", xElement.Element("result").Element("address_component").Element("short_name")
                            .Value),
                    Latitude = string.Format("{0}", xElement.Element("result").Element("geometry").Element("location")
                                .Element("lat")
                                .Value),
                    Longitude = string.Format("{0}", xElement.Element("result").Element("geometry").Element("location")
                                    .Element("lng")
                                    .Value),
                    Country_ID = int.Parse(cid)
                };
            }
            if (xElement.Element("status").Value == "ZERO_RESULTS")
            {
                return new City
                {
                    City_ID = 1
                };
            }
            return new City
            {
                City_ID = 1
            };
        }

        public static string UploadImages(string img1, string img2)
        {
            string text3;
            string text2;
            string text;
            string text4 = text3 = (text2 = (text = ""));
            if (!string.IsNullOrEmpty(img1))
            {
                text4 = StaticCalls.GetName();
                if (!string.IsNullOrEmpty(img2))
                {
                    text3 = StaticCalls.GetName();
                }
                if (HttpContext.Current != null)
                {
                    text2 = HttpContext.Current.Server.MapPath("~/Property_Images/") + text4 + ".jpg";
                    text = HttpContext.Current.Server.MapPath("~/Property_Images/") + text3 + ".jpg";
                }
                else
                {
                    text2 = HostingEnvironment.MapPath("~/Property_Images/") + text4 + ".jpg";
                    text = HostingEnvironment.MapPath("~/Property_Images/") + text3 + ".jpg";
                }
                StaticCalls.send_text("Local Path 1: " + text2);
                StaticCalls.send_text("Local Path 2: " + text);
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        webClient.DownloadFile(img1, text2);
                        StaticCalls.send_text(img1 + " Uoloaded to: " + text2);
                        webClient.DownloadFile(img2, text);
                        StaticCalls.send_text(img2 + " Uoloaded to: " + text2);
                    }
                    catch (Exception)
                    {
                        return "NULL";
                    }
                }
                return text4 + ".jpg|" + ((!string.IsNullOrEmpty(text3)) ? (text3 + ".jpg|") : "");
            }
            return "NULL";
        }

        public static void Move_Images()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo((HttpContext.Current != null) ? HttpContext.Current.Request.PhysicalApplicationPath : AppDomain.CurrentDomain.BaseDirectory);
            DirectoryInfo sourc = new DirectoryInfo(directoryInfo.FullName + "\\Property_Images");
            DirectoryInfo dest = new DirectoryInfo(directoryInfo.Parent.FullName + "\\RealEstate\\PropertyImage");
            FileInfo[] files_source = sourc.GetFiles();
            int source_count = files_source.Length;
            List<string> ls = Get_Property_List();
            foreach (FileInfo fileInfo in files_source)
            {
                if (File.Exists(fileInfo.FullName))
                {
                    if (!HttpContext.Current.Request.Url.Authority.Contains("localhost"))
                    {
                        if (ls.Contains(fileInfo.Name))
                            File.Move(fileInfo.FullName, dest + "\\" + fileInfo.Name);
                    }
                }
                else
                {
                    StaticCalls.send_text("Check Image move Path");
                }
            }
            if (source_count> 0)
            {
                files_source = sourc.GetFiles();
                for (int i = 0; i < files_source.Length; i++)
                    files_source[i].Delete();
            }
            StaticCalls.send_text(source_count + " Images Moved");
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            return new string((from s in Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
                               select s[random.Next(s.Length)]).ToArray());
        }

        public static string GetName()
        {
            string text = StaticCalls.RandomString(15);
            for (int i = 0; i < StaticCalls.imagenames.Count; i++)
            {
                if (text == StaticCalls.imagenames[i])
                {
                    text = StaticCalls.RandomString(15);
                    i = 0;
                }
            }
            StaticCalls.imagenames.Add(text);
            return text;
        }

        private static void InitNames()
        {
            if (StaticCalls.imagenames.Count == 0)
            {
                Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory);
                StaticCalls.imagenames = StaticCalls.GetFilesList(AppDomain.CurrentDomain.BaseDirectory + "Property_Images\\");
            }
        }

        public static List<string> GetFilesList(string path)
        {
            FileInfo[] files = new DirectoryInfo(path).GetFiles();
            List<string> list = new List<string>();
            FileInfo[] array = files;
            foreach (FileInfo fileInfo in array)
            {
                list.Add(fileInfo.Name);
            }
            return list;
        }
    }
}