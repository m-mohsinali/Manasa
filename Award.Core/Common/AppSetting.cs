using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Award.Core.Common
{
    public class AppSetting
    {
        #region Singleton

        private static volatile AppSetting _instance;
        private static readonly object _padLock = new object();

        public static AppSetting Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (_padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppSetting();
                    }
                }
                return _instance;
            }
        }

        private AppSetting()
        {

        }

        #endregion

        #region Paths 

        public readonly string AwardImagePath    =   $"{Path.Combine("Image","Award")}{Path.DirectorySeparatorChar}";
        
        public readonly string AwardDocumentPath =   $"{Path.Combine("Document", "Award")}{Path.DirectorySeparatorChar}";

        public readonly string CategoryDocumentPath = $"{Path.Combine("Document", "Category")}{Path.DirectorySeparatorChar}";

        public readonly string SettingVideoPath = $"{Path.Combine("Video")}{Path.DirectorySeparatorChar}";

        public readonly string AnnouncementImagePath = $"{Path.Combine("Image", "Announcement")}{Path.DirectorySeparatorChar}";

        #endregion

        public string Language { get; set; } = Constants.Languages.ENGLISH;

        public string Direction { get; set; } = Constants.Languages.ENGLISH_LANG_DIR;


    }
}
