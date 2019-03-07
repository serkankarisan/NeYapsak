using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeYapsak.Entity.Identity
{
    public class ApplicationRole : IdentityRole
    {
        private string _description;




        [StringLength(200, ErrorMessage = "İçerik {0} karakterden uzun olmamalıdır!")]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                string BDesc = "";
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Contains(' '))
                    {
                        string[] Desc = value.Split(' ');
                        foreach (string item in Desc)
                        {
                            string d;
                            if (!string.IsNullOrEmpty(item))
                            {
                                d = item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower();

                            }
                            else
                            {
                                d = item;
                            }
                            if (BDesc == "")
                            {
                                BDesc += d;
                            }
                            else
                            {
                                BDesc += " " + d;
                            }
                        }
                    }
                    else
                    {
                        BDesc = value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
                    }
                }
                else
                {
                    BDesc = value;
                }
                _description = BDesc;
            }
        }
    }
}
