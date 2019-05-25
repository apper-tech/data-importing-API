using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace DataImporting
{
    public class Nestoria
    {
        public static string User_ID = "1e98087e-72ed-4e5e-b2ae-df3952599935";

        public static string Country_UK = "77";

        public static string Country_DE = "58";

        public static string Country_FR = "75";

        public static string Country_ES = "68";

        public static void RunTask()
        {
            StaticCalls.send_text("\\c");
            foreach (Setting_Nestoria parameter in Nestoria.Get_Parameters())
            {
                StaticCalls.InitVars();
                string path = Nestoria.LoadList(parameter.Country, parameter.Place_Name, parameter.Listing_Type, parameter.Count, parameter.Keywords, File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Temp/Nestoria.xml") ? (AppDomain.CurrentDomain.BaseDirectory + "/Temp/Nestoria.xml") : File.Create(AppDomain.CurrentDomain.BaseDirectory + "/Temp/Nestoria.xml").Name);
                Nestoria.Write_Log_Txt(path, parameter.Place_Name, parameter.Listing_Type, parameter.Count, parameter.Keywords, parameter.Country, true);
                List<Property_Nestoria> list = Nestoria.ProccessData(path, parameter.Place_Name, parameter.Listing_Type, parameter.Count, parameter.Keywords, parameter.Country);
                StaticCalls.Move_Images();
                Nestoria.Send_Email(Nestoria.WriteListToExcel(list, "/Logs/log " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xls"));
                Nestoria.Write_Log_Txt(path, parameter.Place_Name, parameter.Listing_Type, parameter.Count, parameter.Keywords, parameter.Country, false);
            }
        }

        public static void Send_Email(string logpath)
        {
            DateTime now = DateTime.Now;
            string subj = "Job Report " + now.ToShortDateString();
            string[] obj = new string[5]
            {
            "<h4>Job Report</h4><br>Job Ran successfully on: ",
            null,
            null,
            null,
            null
            };
            now = DateTime.Now;
            obj[1] = now.ToString();
            obj[2] = "<br>the log for the data inserted can be found here <a href='http://data.akaratak.com/log.txt'>Log</a><br> and can be downloaded here <a href='"+logpath+"'>";
            obj[4] = "Download Excel</a><br>this log is generated daily";
            string body = string.Concat(obj);
            StaticCalls.SendLogMail("mcs7dp3@gmail.com", subj, body);
            StaticCalls.SendLogMail("maltabba@gmail.com", subj, body);
            StaticCalls.SendLogMail("mhafez@apper.tech", subj, body);
            StaticCalls.SendLogMail("maltabba@apper.tech", subj, body);
            StaticCalls.send_text("Email Sent : mcs7dp3@gmail.com");
            StaticCalls.send_text("Email Sent : maltabba@gmail.com");
            StaticCalls.send_text("Email Sent : mhafez@apper.tech");
            StaticCalls.send_text("Email Sent : maltabba@apper.tech");
        }

        public static string LoadList(string country, string place_name, string listing_type, string num_res, string keywords, string path)
        {
            return Nestoria.writeTempFile(StaticCalls.URLToString("http://api.nestoria." + country + "/api" + "?action=search_listings" + "&encoding=xml" + "&place_name=" + place_name + "&listing_type=" + listing_type + "&number_of_results=" + num_res + "&keywords=" + keywords), path);
        }

        private static string writeTempFile(string content, string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.Write(content);
            streamWriter.Close();
            fileStream.Close();
            return path;
        }

        private static void Write_Log_Txt(string path, string place_name, string listing_type, string count, string keywords, string country, bool head)
        {
            string[] obj = new string[15]
            {
            "-------------------------------------------------------------------------------------------------------------------------\r\n                             -------------------------------------- ",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
            };
            DateTime now = DateTime.Now;
            obj[1] = now.ToShortDateString();
            obj[2] = "  |  ";
            now = DateTime.Now;
            obj[3] = now.ToShortTimeString();
            obj[4] = " (";
            obj[5] = Nestoria.GetCountry(true, country);
            obj[6] = ", ";
            obj[7] = place_name;
            obj[8] = ", ";
            obj[9] = listing_type;
            obj[10] = ", ";
            obj[11] = count;
            obj[12] = ", ";
            obj[13] = keywords;
            obj[14] = ")------------------------------------------";
            string text = string.Concat(obj);
            string text2 = "------------------------------------------------------------------------------------------------------------------------------------\r\n                              ------------------------------------------------------------------------------------------------------------------------------------";
            if (head)
            {
                StaticCalls.send_text(text);
                StaticCalls.send_text("Sending Requst....");
                StaticCalls.send_text("File Recived: ");
                StaticCalls.send_text(File.ReadAllText(path));
            }
            else
            {
                StaticCalls.send_text(text2);
            }
        }

        private static List<Property_Nestoria> ProccessData(string path, string place_name, string listing_type, string count, string keywords, string country)
        {
            StaticCalls.send_text("Diagnostic Data");
            StaticCalls.send_text("Converting XML to List !");
            List<Property_Nestoria> list = Nestoria.writeXmlToList(path);
            if (list.Count > 0)
            {
                StaticCalls.send_text("-----------------------------------------------------");
                StaticCalls.send_text("Writing List to Database !");
                Nestoria.WriteListToDatabase(list, Nestoria.GetGarden(keywords), Nestoria.GetCity(place_name, country), Nestoria.GetCountry(false, country), Nestoria.GetBuy(listing_type), Nestoria.User_ID, HttpContext.Current);
                StaticCalls.send_text("-----------------------------------------------------");
                StaticCalls.send_text("Done !");
            }
            else
            {
                StaticCalls.send_text("Check Parameters");
            }
            return list;
        }

        public static string WriteListToExcel<T>(List<T> list, string fulllPath)
        {
            try
            {
                List<string> list2 = new List<string>();
                list2.Add(string.Join(string.Empty, from i in typeof(T).GetProperties()
                                                    select string.Format("{0}\t", i.Name)));
                list2.AddRange(from i in (IEnumerable<T>)list
                               select string.Join<object>("\t", Enumerable.Select<PropertyInfo, object>((IEnumerable<PropertyInfo>)i.GetType().GetProperties(), (Func<PropertyInfo, object>)((PropertyInfo t) => t.GetValue((T)i, null)))));
                StaticCalls.send_text("Wrote to Excel");
                File.WriteAllLines(fulllPath, list2);
            }
            catch (Exception)
            {
                StaticCalls.send_text("Check Excel Write");
            }
            return "http://data.akaratak.com/" + fulllPath;
        }

        public static List<Property_Nestoria> writeXmlToList(string path)
        {
            string text = File.ReadAllText(path);
            if (text.Length <= 0)
            {
                StaticCalls.send_text("Check Xml to List");
                return new List<Property_Nestoria>();
            }
            XDocument xDocument = XDocument.Parse(text);
            List<Property_Nestoria> list = new List<Property_Nestoria>();
            foreach (XElement item in xDocument.Root.DescendantNodes().OfType<XElement>())
            {
                if (item.Name == (XName)"listings")
                {
                    try
                    {
                        Property_Nestoria property_Nestoria = new Property_Nestoria();
                        property_Nestoria.bath_n = ((item.Attribute("bathroom_number") != null) ? item.Attribute("bathroom_number").Value : "0");
                        property_Nestoria.bed_n = ((item.Attribute("bedroom_number") != null) ? item.Attribute("bedroom_number").Value : "0");
                        property_Nestoria.car_s = ((item.Attribute("car_spaces") != null) ? item.Attribute("car_spaces").Value : "0");
                        property_Nestoria.comm = ((item.Attribute("datasource_name") != null) ? item.Attribute("datasource_name").Value : "0");
                        property_Nestoria.constr_y = ((item.Attribute("datasource_name") != null) ? item.Attribute("datasource_name").Value : "0");
                        property_Nestoria.data_s = ((item.Attribute("datasource_name") != null) ? item.Attribute("datasource_name").Value : "none");
                        property_Nestoria.floor = ((item.Attribute("floor") != null) ? item.Attribute("floor").Value : "0");
                        property_Nestoria.img_h = ((item.Attribute("img_height") != null) ? item.Attribute("img_height").Value : "0");
                        property_Nestoria.img_u = ((item.Attribute("img_url") != null) ? item.Attribute("img_url").Value : "0");
                        property_Nestoria.img_w = ((item.Attribute("img_width") != null) ? item.Attribute("img_width").Value : "0");
                        property_Nestoria.keywrd = ((item.Attribute("keywords") != null) ? item.Attribute("keywords").Value : "none");
                        property_Nestoria.lat = ((item.Attribute("latitude") != null) ? item.Attribute("latitude").Value : "0");
                        property_Nestoria.lister_u = ((item.Attribute("lister_url") != null) ? item.Attribute("lister_url").Value : "0");
                        property_Nestoria.list_t = ((item.Attribute("listing_type") != null) ? item.Attribute("listing_type").Value : "buy");
                        property_Nestoria.loc_a = ((item.Attribute("location_accuracy") != null) ? item.Attribute("location_accuracy").Value : "0");
                        property_Nestoria.lng = ((item.Attribute("longitude") != null) ? item.Attribute("longitude").Value : "0");
                        property_Nestoria.price = ((item.Attribute("price") != null) ? item.Attribute("price").Value : "0");
                        property_Nestoria.price_c = ((item.Attribute("price_formatted") != null) ? item.Attribute("price_formatted").Value : "$");
                        property_Nestoria.price_f = ((item.Attribute("price_formatted") != null) ? item.Attribute("price_formatted").Value : "0");
                        property_Nestoria.price_h = ((item.Attribute("price_currency") != null) ? item.Attribute("price_currency").Value : "0");
                        property_Nestoria.price_l = ((item.Attribute("price_currency") != null) ? item.Attribute("price_currency").Value : "0");
                        property_Nestoria.price_t = ((item.Attribute("price_currency") != null) ? item.Attribute("price_currency").Value : "0");
                        property_Nestoria.property_t = ((item.Attribute("property_type") != null) ? item.Attribute("property_type").Value : "house");
                        property_Nestoria.size = ((item.Attribute("size") != null) ? item.Attribute("size").Value : "0");
                        property_Nestoria.size_t = ((item.Attribute("size_type") != null) ? item.Attribute("size_type").Value : "0");
                        property_Nestoria.summary = ((item.Attribute("summary") != null) ? item.Attribute("summary").Value : "none");
                        property_Nestoria.thumb_h = ((item.Attribute("thumb_height") != null) ? item.Attribute("thumb_height").Value : "0");
                        property_Nestoria.thumb_u = ((item.Attribute("thumb_url") != null) ? item.Attribute("thumb_url").Value : "0");
                        property_Nestoria.thumb_w = ((item.Attribute("thumb_width") != null) ? item.Attribute("thumb_width").Value : "0");
                        property_Nestoria.title = ((item.Attribute("title") != null) ? item.Attribute("title").Value : "none");
                        property_Nestoria.updated_d = ((item.Attribute("updated_in_days") != null) ? item.Attribute("updated_in_days").Value : "0");
                        property_Nestoria.updated_d_f = ((item.Attribute("updated_in_days_formatted") != null) ? item.Attribute("updated_in_days_formatted").Value : "0");
                        property_Nestoria.url = ((item.Attribute("lister_url") != null) ? item.Attribute("lister_url").Value : "none");
                        property_Nestoria.pr_id = ((item.Attribute("lister_url") != null) ? Nestoria.GetId(item.Attribute("lister_url").Value) : "0");
                        list.Add(property_Nestoria);
                    }
                    catch (Exception)
                    {
                        StaticCalls.send_text("Check Xml to List");
                    }
                }
            }
            StaticCalls.send_text("Wrote Xml to List");
            return list;
        }

        public static string GetId(string url)
        {
            return Regex.Match(url, "(?<=l/)\\d+(?=/t)").Value;
        }

        public static void WriteListToDatabase(List<Property_Nestoria> list, string garden, string city_id, string country_id, bool buy, string userID, HttpContext context)
        {
            List<string> list2 = null;
            if (context != null)
            {
                list2 = (context.Session["idlist"] as List<string>);
            }
            if (list2 == null)
            {
                list2 = StaticCalls.GetPropertyIDList();
                if (context != null)
                {
                    context.Session.Add("idlist", list2);
                }
            }
            List<string> list3 = list2;
            List<Property> list4 = new List<Property>();
            foreach (Property_Nestoria item in list)
            {
                Property property = Nestoria.NestoriaToAkaratak(item, garden, city_id, country_id, userID, buy);
                property.Property_Photo = StaticCalls.UploadImages(item.img_u, item.thumb_u);
                if (list3.Contains(item.pr_id))
                {
                    StaticCalls.UpdateProperty(property);
                }
                else
                {
                    list4.Add(property);
                }
                StaticCalls.send_text("p Added =" + property.Property_Id_ext);
            }
            StaticCalls.InsertProperties(list4);
        }

        public static Property NestoriaToAkaratak(Property_Nestoria item, string garden, string city_id, string country_id, string userid, bool buy)
        {
            Property property = new Property();
            property.Property_Category_ID = 1;
            property.Property_Type_ID = 1;
            property.Property_Size = int.Parse(item.size);
            property.Date_Added = DateTime.UtcNow;
            property.Floor = int.Parse(item.floor);
            property.Has_Garden = ((byte)((garden == "1") ? 1 : 0) != 0);
            property.Has_Garage = ((byte)((item.car_s.Length > 0) ? 1 : 0) != 0);
            property.Num_Bedrooms = int.Parse(item.bed_n);
            property.Num_Bathrooms = int.Parse(item.bath_n);
            property.Expire_Date = DateTime.UtcNow.AddMonths(3);
            property.Contract_Type = 1;
            property.City_ID = int.Parse(city_id);
            property.Country_ID = int.Parse(country_id);
            property.Address = item.title;
            property.Location = item.lat + "," + item.lng + "," + item.loc_a;
            property.Zip_Code = "";
            property.Other_Details = item.summary + "\n" + item.keywrd;
            try
            {
                property.Sale_Price = (buy ? Convert.ToInt32(item.price.Substring(0, item.price.Length - 3)) : 0);
                property.Rent_Price = ((!buy) ? Convert.ToInt32(item.price.Substring(0, item.price.Length - 3)) : 0);
            }
            catch
            {
                property.Sale_Price = 0;
                property.Rent_Price = 0;
            }
            property.Num_Floors = null;
            property.User_ID = userid;
            property.Url_ext = item.lister_u;
            property.Property_Id_ext = item.pr_id;
            return property;
        }

        public static List<Setting_Nestoria> Get_Parameters()
        {
            DataTable query = StaticCalls.GetQuery("SELECT        Nestoria.place, Nestoria.listing, Nestoria.count, Country.country_code, Nestoria.keywords\r\n                           FROM            Country INNER JOIN\r\n                           Nestoria ON Country.country_id = Nestoria.country_id", "SettingsConnectionString", null);
            List<Setting_Nestoria> list = new List<Setting_Nestoria>();
            if(query.Rows!=null)
            foreach (DataRow row in query.Rows)
            {
                list.Add(new Setting_Nestoria
                {
                    Place_Name = row["place"].ToString(),
                    Country = row["country_code"].ToString(),
                    Keywords = row["keywords"].ToString(),
                    Listing_Type = row["listing"].ToString(),
                    Count = row["count"].ToString()
                });
            }
            return list;
        }

        private static string GetGarden(string keywords)
        {
            if (!keywords.Contains("Garden"))
            {
                return "0";
            }
            return "1";
        }

        private static string GetCountry(bool r, string country)
        {
            if (!r)
            {
                if (!(country == "co.uk"))
                {
                    if (!(country == "de"))
                    {
                        if (!(country == "es"))
                        {
                            if (country == "fr")
                            {
                                return Nestoria.Country_FR;
                            }
                            goto IL_009f;
                        }
                        return Nestoria.Country_ES;
                    }
                    return Nestoria.Country_DE;
                }
                return Nestoria.Country_UK;
            }
            if (!(country == "co.uk"))
            {
                if (!(country == "de"))
                {
                    if (!(country == "es"))
                    {
                        if (country == "fr")
                        {
                            return "FR";
                        }
                        goto IL_009f;
                    }
                    return "ES";
                }
                return "DE";
            }
            return "GB";
            IL_009f:
            return "1";
        }

        private static string GetCity(string place_name, string country)
        {
            foreach (City city in StaticCalls.GetCityList(Nestoria.GetCountry(false, country)))
            {
                if (city.City_Name == place_name)
                {
                    return city.City_ID.ToString();
                }
            }
            return Nestoria.InsertCity(place_name, country);
        }

        private static string InsertCity(string city_Name, string country)
        {
            return StaticCalls.InsertCity(StaticCalls.GetCityData(Nestoria.GetCountry(true, country), city_Name, Nestoria.GetCountry(false, country)));
        }

        private static bool GetBuy(string type)
        {
            if (type.Contains("buy"))
            {
                return true;
            }
            return false;
        }
    }
}