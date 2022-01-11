using System.Data;

namespace Award.Core.Common
{
    public class Constants
    {
        public static class Roles
        {
            public const string ADMINISTRATOR = "Administrator";
            public const string QUALITY_SECTION_MANAGER = "QualitySectionManager";
            public const string END_USER = "EndUser";
            public const string AUDIT_MANAGER = "AuditManager";
            public const string AUDITOR = "Auditor";
            public const string JURY  = "Jury";
        }

        public class Languages
        {
            public const string ARABIC_LANG_DIR = "rtl";
            public const string ENGLISH_LANG_DIR = "ltr";

            public const string ARABIC = "ar";
            public const string ENGLISH = "en";
        }
        public class Colors
        {
            public string ColorValue { get; set; }

            public const string First_Color = "#006391";
            public const string Second_Color = "#007ea9";
            public const string Third_Color = "#0099b3";
            public const string Fourth_Color = "#00b4af";
            public const string Fifth_Color = "#00cc9d";
            public const string Sixth_Color = "#58e282";
            //public const string First_Color = "ltr";#adf464
            //public const string First_Color = "ltr";#feff4d
            //public const string First_Color = "ltr";
            //public const string First_Color = "ltr";
        }

        public DataTable GetColors()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Color", System.Type.GetType("System.String"));

            DataRow dr = dt.NewRow();
            dr["Color"] = "#006391";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Color"] = "#007da2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Color"] = "#00969e";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Color"] = "#008d7e";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Color"] = "#00a570";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Color"] = "#69ba59";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Color"] = "#b2cb43";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Color"] = "#ffd43d";
            dt.Rows.Add(dr);

            return dt;
        }



    }
}
