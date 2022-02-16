using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using ALCS_Library.ALCS_Basics;
using ALCS_Library.ALCS_Data;
using ALCS_Library.ALCS_Data.ALCS_SQLWork;
using ALCS_Library.ALCS_Format;
using ALCS_Library.ALCS_JavaScript;
using ALCS_Library.ALCS_Menu;
using ALCS_Library.ALCS_WWW;
using ALCS_Library.ALCS_WWW.ALCS_WWWControls;

public partial class DefaultIntegrated : System.Web.UI.Page 
{
    string alertText = "";

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string contractNo = ALCS_WWWTools.ReadSubmittedValue(this.txt_ContractNo, "");
        int contractID = ALCS_WWWTools.ReadSubmittedValue(this.txt_ContractNo, 0);
        string regoNo = ALCS_WWWTools.ReadSubmittedValue(this.txt_CarRego, "");

        regoNo = regoNo.Replace(" ", "").Replace("-", "");

        if((contractNo == "") && (regoNo == ""))
        {
            this.alertText = "Please eneter 'Contract No' OR 'Car Rego' ";
            return;
        }

        if ((contractNo != "") && (contractID == 0))
        {
            this.alertText = "Please contract No is invalid";
            return;
        }

        // query ....
        string sqlStr = "";

        if (contractID > 0)
        {
            sqlStr = "SELECT * FROM tblContract where (CN_ID =" + contractID.ToString() + ")";
        }
        else if (regoNo != "")
        {
            sqlStr = "SELECT * FROM tblContract where (REPLACE(REPLACE(CN_RegNo, '-', ''), ' ', '') LIKE '%" + regoNo + "%')";
        }

        // query ...
        ALCS_QueryExecuter qe = new ALCS_QueryExecuter();
        DataTable dt = new DataTable();

        qe.LoadDataTable(sqlStr, ref dt);

        if(!qe.isHealthy)
        {
            this.alertText = "Error occured while extracting data";
        }
        else
        {
            int rc = dt.Rows.Count;

            if(rc == 1)
            {
                this.alertText = "1 record found";
            }
            else
            {
                 this.alertText = dt.Rows.Count.ToString() + " records found";
            }
        }
        

        
    }

    protected void Page_Prerender(object sender, EventArgs e)
    {

        ALCS_WWWTools.ClientFilterInput(this.txt_ContractNo, Ctrl_InputType.Integer);

        // Script Injection.
        ClientScriptManager csm = Page.ClientScript;
        string varStr = "";

        varStr += ScriptInjector.DeclareJSVar(this.txt_ContractNo);
        varStr += ScriptInjector.DeclareJSVar(this.txt_CarRego);

        varStr += ScriptInjector.DeclareJSVar("eText", this.alertText);


        //Expose the string 
        csm.RegisterStartupScript(this.GetType(), "AAP_LOCATION_WATCH", varStr, true);

        
    }

}
