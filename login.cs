static void LoginWEB2(string user)
            {
                try
                {
                // multi login from file
                    foreach (var line in File.ReadLines("Accounts.txt"))
                    {

                        var splitedUsername = line.Split(':')[0].Split(' ')[0];
                        var splitedPassword = line.Split(':')[1].Split(' ')[0];

                        string Data = $"username={splitedUsername}&enc_password=#PWD_INSTAGRAM_BROWSER:0:1589682409:{splitedPassword}";
                        HttpWebRequest WEBREQUEST = (HttpWebRequest)WebRequest.Create("https://www.instagram.com/accounts/login/ajax/");
                        WEBREQUEST.Method = "POST";
                        WEBREQUEST.Host = "www.instagram.com";
                        WEBREQUEST.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36";
                        WEBREQUEST.ContentType = "application/x-www-form-urlencoded";
                        WEBREQUEST.Headers.Add("x-csrftoken", "missing");
                        WEBREQUEST.Headers.Add("accept-language", "en-US,en;q=0.9,ar-SA;q=0.8,ar;q=0.7");
                        WEBREQUEST.Headers.Add("cookie", "mid=YBFqywABAAE0ZL2yz_x6XtohlZPN; csrftoken=YVjsmppWJc6ylI1luYyrMOPTlfIfGXva;");
                        WEBREQUEST.Headers.Add("x-instagram-ajax", "1");
                        WEBREQUEST.Headers.Add("x-requested-with", "XMLHttpRequest");
                        WEBREQUEST.KeepAlive = true;
                        WEBREQUEST.Timeout = 120000;
                        WEBREQUEST.CookieContainer = CookieContainer;
                        WEBREQUEST.ProtocolVersion = HttpVersion.Version11;
                        WEBREQUEST.ServicePoint.UseNagleAlgorithm = false;
                        WEBREQUEST.ServicePoint.Expect100Continue = false;
                        byte[] bytes = Encoding.ASCII.GetBytes(Data);
                        WEBREQUEST.ContentLength = (long)bytes.Length;
                        Stream Stream = WEBREQUEST.GetRequestStream();
                        Stream.Write(bytes, 0, bytes.Length);
                        Stream.Flush();
                        Stream.Close();
                        Stream.Dispose();
                        HttpWebResponse Resp;
                        try
                        {
                            Resp = (HttpWebResponse)WEBREQUEST.GetResponse();
                        }
                        catch (WebException ex)
                        {
                            Resp = (HttpWebResponse)ex.Response;
                        }
                        StreamReader StreamReader = new StreamReader(Resp.GetResponseStream());
                        string Response = StreamReader.ReadToEnd().ToString();
                        bool DoneLogin = Response.Contains("\"authenticated\":true,\"");
                        bool Secure = Response.Contains("checkpoint_required");
                        bool bad = Response.Contains("\"authenticated\":false,\"");
                        bool idk = Response.Contains("errors");
                        bool Spammed = Response.Contains("wait") || Response.Contains("spam");
                        if (DoneLogin)
                        {
                            MessageBox.Show($"Done Logged In... {splitedUsername}", "InstaSharp", 3000);
                            string IDsess = Convert.ToString(Resp.Cookies["sessionid"]);
                            ard = IDsess.Replace("sessionid=", string.Empty);
                            // extracts the sessionID that we will need to manipulate with instagram
                        }
                        else if (Secure)
                        {
                            MessageBox.Show($"Checkpoint required... {splitedUsername}", "InstaSharp");
                        }
                        else if (Spammed)
                        {
                            MessageBox.Show($"Spam on account {splitedUsername}...", "InstaSharp");
                        }
                        else if (bad)
                        {
                            MessageBox.Show($"Incorrect Password on {splitedUsername}...", "InstaSharp");
                        }
                        else if (idk)
                        {
                            MessageBox.Show("Sorry, there was a problem with your request", "InstaSharp");
                    }
                        else
                        {
                            MessageBox.Show($"Error on account {splitedUsername}...", "InstaSharp");
                            MessageBox.Show($"Error: {Response}...", "InstaSharp");
                        }
                        StreamReader.Dispose();
                        StreamReader.Close();
                    }
                    MessageBox.Show("Logged In Successfully");
                    Thread.Sleep(10000);
                }
                catch (Exception)
                {
                }
            }
