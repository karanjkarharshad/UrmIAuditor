using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrmIAuditor.Models
{


    public class AuditDetails
    {
        public string template_id { get; set; }
        public string audit_id { get; set; }
        public bool archived { get; set; }
        public string created_at { get; set; }
        public string modified_at { get; set; }
        public Audit_Data audit_data { get; set; }
        public Template_Data template_data { get; set; }
        public Header_Items[] header_items { get; set; }
        public Header_Items[] items { get; set; }
        public string audit_url { get; set; }
    }

    public class Audit_Data
    {
        public double score { get; set; }
        public double total_score { get; set; }
        public double score_percentage { get; set; }
        public string name { get; set; }
        public double duration { get; set; }
        public Location location { get; set; }
        public Authorship authorship { get; set; }
        public string date_completed { get; set; }
        public string date_modified { get; set; }
        public string date_started { get; set; }
    }

    public class Authorship
    {
        public string device_id { get; set; }
        public string owner { get; set; }
        public string owner_id { get; set; }
        public string author { get; set; }
        public string author_id { get; set; }
    }

    public class Template_Data
    {
        public Authorship authorship { get; set; }
        public Metadata metadata { get; set; }
        public Response_Sets response_sets { get; set; }
    }

    public class Metadata
    {
        public string description { get; set; }
        public string name { get; set; }
        public Image image { get; set; }
    }

    public class Response_Sets
    {
        public _62C11f9849B54D8886B0193C87c2b373 _62c11f9849b54d8886b0193c87c2b373 { get; set; }
        public _6EF7C9D55B2E42559709EA12501DD8A6 _6EF7C9D55B2E42559709EA12501DD8A6 { get; set; }
        public Responseset_732Cab984b394ea6a8ed923e4ff4f0e0 responseset_732cab984b394ea6a8ed923e4ff4f0e0 { get; set; }
    }

    public class _62C11f9849B54D8886B0193C87c2b373
    {
        public string id { get; set; }
        public string type { get; set; }
        public Respons[] responses { get; set; }
    }

    public class Respons
    {
        public string id { get; set; }
        public string label { get; set; }
        public string colour { get; set; }
        public int score { get; set; }
        public string short_label { get; set; }
        public string type { get; set; }
        public bool enable_score { get; set; }
    }

    public class _6EF7C9D55B2E42559709EA12501DD8A6
    {
        public string id { get; set; }
        public string type { get; set; }
        public Respons[] responses { get; set; }
    }

   

    public class Responseset_732Cab984b394ea6a8ed923e4ff4f0e0
    {
        public string id { get; set; }
        public string type { get; set; }
        public Respons[] responses { get; set; }
    }

    

    public class Header_Items
    {
        public string item_id { get; set; }
        public string parent_id { get; set; }
        public string label { get; set; }
        public string type { get; set; }

        public Options options { get; set; }
        public Responses responses { get; set; }
        public Image[] media { get; set; }
        public string[] children { get; set; }       
        public Scoring scoring { get; set; }



    }

    public class Options
    {



        public string condition { get; set; }
        public string element { get; set; }
        public bool enable_date { get; set; }
        public bool enable_signature_timestamp { get; set; }
        public bool enable_time { get; set; }
        public bool hide_barcode { get; set; }
        public string increment { get; set; }
        public bool is_mandatory { get; set; }
        public string label { get; set; }
        public string link { get; set; }
        public bool locked { get; set; }
        public string max { get; set; }
        public Image media { get; set; }
        public string min { get; set; }
        public bool multiple_selection { get; set; }
        public bool required { get; set; }
        public string response_set { get; set; }
        public string[] failed_responses { get; set; }
        public string secure { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string[] values { get; set; }
        public bool visible_in_audit { get; set; }
        public bool visible_in_report { get; set; }
        public string weighting { get; set; }





    }

    public class Responses
    {




        public string text { get; set; }
        public string value { get; set; }
        public string name { get; set; }
        public string timestamp { get; set; }
        public string datetime { get; set; }
        public string location_text { get; set; }
        public Location location { get; set; }
        public Respons[] selected { get; set; }
        public bool failed { get; set; }
        public Weather weather { get; set; }
        public Image[] media { get; set; }
        public Image image { get; set; }







    }

    public class Scoring
    {
        public int combined_score { get; set; }
        public int combined_max_score { get; set; }
        public double combined_score_percentage { get; set; }
        public int score { get; set; }
        public int max_score { get; set; }
        public double score_percentage { get; set; }
    }

    public class Location
    {
       


        public string administrative_area { get; set; }
        public string country { get; set; }
        public string[] formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string iso_country_code { get; set; }
        public string locality { get; set; }
        public string name { get; set; }
        public string postal_code { get; set; }
        public string sub_administrative_area { get; set; }
        public string sub_locality { get; set; }
        public string sub_thoroughfare { get; set; }
        public string thoroughfare { get; set; }




    }

    public class Geometry
    {
        public string type { get; set; }
        public double[] coordinates { get; set; }
    }


    public class Started
    {
        public double accuracy { get; set; }
        public Geometry geometry { get; set; }
    }













    public class Image
    {
        public string date_created { get; set; }
        public string file_ext { get; set; }
        public string label { get; set; }
        public string media_id { get; set; }
        public string href { get; set; }
    }

    



  



    

    public class Weather
    {

    }



    //public class Selected
    //{
    //    public string id { get; set; }
    //    public string label { get; set; }
    //    public string colour { get; set; }
    //    public int score { get; set; }
    //    public string short_label { get; set; }
    //    public string type { get; set; }
    //    public bool enable_score { get; set; }
    //}



}