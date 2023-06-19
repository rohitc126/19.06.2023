using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
//using System.Collections.Generic;
//using System.Web.Script.Serialization;
//using System.Web.Script.Services;
//using System.Web.Services;

public partial class HRM_DashBoard_FinanceMenu : System.Web.UI.Page
{
    BAL_EmployeeDefaultAccess fin = new BAL_EmployeeDefaultAccess();
    public DataTable dt;
    public DataView dv;
    BALSGX_SelectCommonMaster common = new BALSGX_SelectCommonMaster();
    BALSGX_VendorMaster master = new BALSGX_VendorMaster();
    BALSGX_AdvanceReceipt advance = new BALSGX_AdvanceReceipt();
    BAL_FA_CBSLocking CBS = new BAL_FA_CBSLocking();
  
    BALSGX_CustomerMaster custMaster = new BALSGX_CustomerMaster();
    Message msg = new Message();


    //---- Added By   : Ashish Kalsarpe
    //---- Added Date : 27/06/2017
    protected void Page_Init(object sender, EventArgs e)
    {
        DataTable dtContact = new DataTable();
        dtContact.Columns.Add("SR", typeof(int));

        //GST Details
        dtContact.Columns.Add("AddressType", typeof(string));
        dtContact.Columns.Add("AddressType_Name", typeof(string));
        dtContact.Columns.Add("DefaultAddress", typeof(int));
        dtContact.Columns.Add("Address1", typeof(string));
     
        dtContact.Columns.Add("Country", typeof(string));
        dtContact.Columns.Add("State", typeof(string));
        dtContact.Columns.Add("StateName", typeof(string));
        dtContact.Columns.Add("City", typeof(string));
        dtContact.Columns.Add("GSTNo", typeof(string));
        dtContact.Columns.Add("Pin", typeof(string));
        dtContact.Columns.Add("PhoneNo_GST", typeof(string));
        dtContact.Columns.Add("FaxNo_GST", typeof(string));
        dtContact.Columns.Add("EmailId", typeof(string));
        dtContact.Columns.Add("WebSite", typeof(string));
        dtContact.Columns.Add("PAN", typeof(string));
        dtContact.Columns.Add("TAN", typeof(string));

        //Contact Details
        dtContact.Columns.Add("Person", typeof(string));
        dtContact.Columns.Add("Designation", typeof(string));
        dtContact.Columns.Add("Department", typeof(string));
        dtContact.Columns.Add("Email", typeof(string));
        dtContact.Columns.Add("PhoneNo", typeof(string));
        dtContact.Columns.Add("FaxNo", typeof(string));
        dtContact.Columns.Add("Mobile", typeof(string));
        dtContact.Columns.Add("Status", typeof(string));
        dtContact.Columns.Add("VENDORCID", typeof(string));
        dtContact.PrimaryKey = new DataColumn[] { dtContact.Columns["SR"] };
        ViewState["dtContact"] = dtContact;

    }

    //------- Ashish Kalsarpe End ---------------------


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnSubmit.Visible = false;
           // lblGSTINExist.Visible = false;
            if (Page.PreviousPage != null)
            {
                ContentPlaceHolder cntPlHol = (ContentPlaceHolder)Page.PreviousPage.Master.FindControl("ContentPlaceHolder1");
                DropDownList DDLCompany = (DropDownList)cntPlHol.FindControl("DDLCompany");
                DropDownList DDLLocation = (DropDownList)cntPlHol.FindControl("DDLLocation");
                HiddenField hdnC_date = (HiddenField)cntPlHol.FindControl("hdnC_date");

                lblCom.Text = DDLCompany.SelectedItem.ToString();
                lblBranch.Text = DDLLocation.SelectedItem.ToString();
                lblTdate.Text = hdnC_date.Value;

                ListOfDays(lblTdate.Text);
                hiddcomp.Value = DDLCompany.SelectedValue.ToString();
                hiddbranch.Value = DDLLocation.SelectedValue.ToString();

                hdn_STATEID.Value = fin.Get_STATE_ID(hiddbranch.Value);


                FILL_Industry();
                FILL_Branch();
                Fill_VendorType();
                //FILL_DDLCity();
               // FILL_DDLState("0");
                FILL_DDLCountry();
                Fill_State();
                Fill_City("0");

                Fill_AddressType_Mst();


                Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());

                lblCBSBranch.Text = DDLLocation.SelectedItem.ToString();
                lblCBSDate.Text = hdnC_date.Value;
                lblCloseBy.Text = Session["EmpName"].ToString();
                /*
                DataTable dtbank = advance.SelectCashBank_CBS(lblCBSDate.Text, "", "B", hiddcomp.Value, hiddbranch.Value);
                if (dtbank.Rows.Count > 0)
                {
                    GVBankDetails.DataSource = dtbank;
                    GVBankDetails.DataBind();
                }

                DataTable dtCash = advance.SelectCashBank_CBS(lblCBSDate.Text, hiddbranch.Value, "C", hiddcomp.Value, hiddbranch.Value);
                if (dtCash.Rows.Count > 0)
                {
                    lblCashClosing.Text = dtCash.Rows[0]["OpBal"].ToString();
                } */
            }
            else
            {//Added By Pramesh Kumar Vishwakarma
                lblCom.Text = Request.QueryString["c"].ToString();
                lblBranch.Text = Request.QueryString["b"].ToString();
                lblTdate.Text = Request.QueryString["d"].ToString(); 

                ListOfDays(lblTdate.Text);
                hiddcomp.Value = Request.QueryString["cId"].ToString();
                hiddbranch.Value = Request.QueryString["bId"].ToString();

                hdn_STATEID.Value = fin.Get_STATE_ID(hiddbranch.Value);

                FILL_Industry();
                FILL_Branch();
                Fill_VendorType();
               // FILL_DDLCity();
               // FILL_DDLState("0");
                FILL_DDLCountry();
                Fill_State();
                Fill_City("0");

                Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());

                lblCBSBranch.Text = Request.QueryString["b"].ToString();
                lblCBSDate.Text = Request.QueryString["d"].ToString(); 
                lblCloseBy.Text = Session["EmpName"].ToString();
               
            /*    DataTable dtbank = advance.SelectCashBank_CBS(lblCBSDate.Text, "", "B", hiddcomp.Value, hiddbranch.Value);
                if (dtbank.Rows.Count > 0)
                {
                    GVBankDetails.DataSource = dtbank;
                    GVBankDetails.DataBind();
                }

                DataTable dtCash = advance.SelectCashBank_CBS(lblCBSDate.Text, hiddbranch.Value, "C", hiddcomp.Value, hiddbranch.Value);
                if (dtCash.Rows.Count > 0)
                {
                    lblCashClosing.Text = dtCash.Rows[0]["OpBal"].ToString();
                } */

                //End
            }
        }
        DataTable dt1 = fin.FinanceDashBoard(Session["Employee_Code"].ToString(), Request.QueryString["Menu"].ToString());
        dt = dt1.Copy();
        dv = dt1.DefaultView;
        dv.RowFilter = "CType='P'";

    }

    void Page_PreRender(object obj, EventArgs e)
    {
        ViewState["update"] = Session["update"];
    }


    private void Fill_City(string State_ID)
    {
        DataTable dtCity = common.Select_City_Mst(State_ID);
        DDLCity.ClearSelection();
        DDLCity.Items.Clear();
        if (dtCity.Rows.Count > 0)
        {
            txtCity.Text = "";
            txtCity.Visible = false;
            DDLCity.Visible = true;
            DDLCity.DataSource = dtCity;
            DDLCity.DataTextField = "city_Name";
            DDLCity.DataValueField = "city_code";
            DDLCity.DataBind();
            DDLCity.Items.Insert(0, new ListItem("Select City", "0"));
            DDLCity.Items.Insert(dtCity.Rows.Count + 1, "Select Others");
        }
        else
        {
            DDLCity.Items.Insert(0, new ListItem("Select City", "0"));
        }
    }

    private void Fill_State()
    {
        DataTable dtState = common.SelectState("0");
        DDLState.ClearSelection();
        DDLState.Items.Clear();
        if (dtState.Rows.Count > 0)
        {
            DDLState.DataSource = dtState;
            DDLState.DataTextField = "State_Name";           
            DDLState.DataValueField = "State_ID";
            DDLState.DataBind();
            DDLState.Items.Insert(0, new ListItem("Select State", "0"));           
        }
        else
        {
            DDLState.DataTextField = "State_Name";
            DDLState.DataValueField = "State_ID";
            DDLState.DataBind();
            DDLState.Items.Insert(0, new ListItem("Select State", "0"));
        }
    }


    /*
    public void FILL_DDLCity()
    {
        DataTable dtCity = common.SelectCity();
        if (dtCity.Rows.Count > 0)
        {
            DDLCity.DataSource = dtCity;

            DDLCity.DataTextField = "city_Name";
            DDLCity.DataValueField = "city_code";
            DDLCity.DataBind();
            DDLCity.Items.Insert(0, "Select City");
            DDLCity.Items.Insert(dtCity.Rows.Count + 1, "Select Others");
        }
        else
        {
            DDLCity.Items.Insert(0, "Select City");
        }

    }

    public void FILL_DDLState(string City_Code)
    {
        DataTable dtState = common.SelectState(City_Code);
        if (dtState.Rows.Count > 0)
        {
            DDLState.DataSource = dtState;
            if (City_Code == "0")
            {
                DDLState.DataTextField = "State_Name";
                //DDLState.DataValueField = "State_Code";
                DDLState.DataValueField = "State_ID";
                DDLState.DataBind();
                DDLState.Items.Insert(0, new ListItem("Select State", "0"));
            }
            else
            {
                DDLState.DataTextField = "State_Name";
                //DDLState.DataValueField = "State_Code";
                DDLState.DataValueField = "State_ID";
                DDLState.DataBind();
            }
        }
    }
    */
    public void FILL_DDLCountry()
    {
        DataTable dtCountry = common.SelectCountry();

        DataView dv = dtCountry.DefaultView;
        dv.RowFilter = "Country_Code='IN '";
        if (dtCountry.Rows.Count > 0)
        {
            DDLCountry.DataSource = dv.ToTable();
            DDLCountry.DataTextField = "Country_Name";
            DDLCountry.DataValueField = "Country_Code";
            DDLCountry.DataBind();
            DDLCountry.Items.Insert(0, new ListItem("Select Country", "0"));
        }
        else
        {
            DDLCountry.Items.Insert(0, new ListItem("Select Country", "0"));        
        }
    }
    public void FILL_Industry()
    {
        DataTable dtInd = common.Fill_Industry();
        if (dtInd.Rows.Count > 0)
        {
            DDLIndustry.DataSource = dtInd;

            DDLIndustry.DataTextField = "Short_Desc";
            DDLIndustry.DataValueField = "Industry_Code";
            DDLIndustry.DataBind();
            DDLIndustry.Items.Insert(0, new ListItem("Select Industry", "0"));
        }
    }
    public void Fill_VendorType()
    {
        DataTable dt = master.Fill_VendorType();

        DDLVendorType.ClearSelection();
        DDLVendorType.Items.Clear();

        if (dt.Rows.Count > 0)
        {
            DDLVendorType.DataSource = dt;
            DDLVendorType.DataTextField = "venTypeDec";
            DDLVendorType.DataValueField = "venType";
            DDLVendorType.DataBind();
            DDLVendorType.Items.Insert(0, new ListItem("Select Vendor Type", "0"));
        }
    }
    public void FILL_Branch()
    {
        DataTable dt = common.SelectLocation("Select Product", "Select Location", Session["Employee_Code"].ToString());
        if (dt.Rows.Count > 0)
        {
            DDLBranch.DataSource = dt;
            DDLBranch.DataTextField = "locationname";
            DDLBranch.DataValueField = "locationCode";
            DDLBranch.DataBind();
            DDLBranch.Items.Insert(0, new ListItem("Select Branch", "0"));
        }
        else 
        {
            DDLBranch.Items.Insert(0, new ListItem("Select Branch", "0"));        
        }
    }
    public void Fill_AddressType_Mst()
    {
        DataTable dtAddress = custMaster.Select_AddressType_Mst();
        ddlAddressType.ClearSelection();
        ddlAddressType.Items.Clear();
        if (dtAddress.Rows.Count > 0)
        {
            ddlAddressType.DataSource = dtAddress;
            ddlAddressType.DataTextField = "Short_Title";
            ddlAddressType.DataValueField = "AddressType_Code";
            ddlAddressType.DataBind();
            ddlAddressType.Items.Insert(0, new ListItem("Select Address Type", "0"));
        }
        else
        {
            ddlAddressType.Items.Insert(0, new ListItem("Select Address Type", "0"));
        }
    }

    private void ListOfDays(string p)
    {
        string[] dates = p.Split('/');
        int noOfdays = DateTime.DaysInMonth(Convert.ToInt32(dates[2]), Convert.ToInt32(dates[1]));
        var source = new Dictionary<string, string>();
        for (int i = 1; i <= noOfdays; i++)
        {
            string date = i + "/" + dates[1] + "/" + dates[2];
            source.Add(date.PadLeft(10, '0'), date.PadLeft(10, '0'));
        }
        lstDate.DataSource = source;
        lstDate.DataTextField = "Key";
        lstDate.DataValueField = "Value";
        lstDate.DataBind();
        lstDate.Items.FindByText(p).Selected = true;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int DefaultCount = 0;

        DataTable dtContact = (DataTable)ViewState["dtContact"];
        if (dtContact.Rows.Count > 0)
        {
            foreach (DataRow dr in dtContact.Rows)
            {
                if ((int)dr["DefaultAddress"] == Convert.ToInt32( "1"))
                {
                    DefaultCount = DefaultCount + 1;
                }            
            } 
        }

        if (DefaultCount > 0)
        {

            string City = "", industry = "";
            if (DDLIndustry.SelectedIndex == 0)
            {
                industry = "";
            }
            else
            {
                industry = DDLIndustry.SelectedValue;
            }
            if (DDLCity.SelectedItem.Text == "Select Others" && txtCity.Text != "")
            {
                City = master.Insert_City_Mst(txtCity.Text, DDLState.SelectedValue.ToString());
                if (City == "0")
                {
                    DDLCity.Visible = true;
                    txtCity.Visible = false;
                    return;
                }
            }
            else
            {
                if (DDLCity.SelectedIndex == 0)
                {
                    City = "";
                }
                else
                {
                    City = DDLCity.SelectedValue.ToString();
                }
            }

            string organizationType = "";
            if (ddlOrganizationType.SelectedValue == "Others(Specify)")
            {
                organizationType = txtOther.Text;
            }
            else
            {
                organizationType = ddlOrganizationType.SelectedValue;
            }

            string result = "";
            if (Session["update"].ToString() == ViewState["update"].ToString())
            {
              
                if (dtContact.Rows.Count > 0)
                {
                    result = master.InsertVendorBrief(txtVendorName.Text, industry, DDLBranch.SelectedValue, Convert.ToDouble(DDLVendorType.SelectedValue),
                       DDLVendorType.SelectedItem.ToString(), txtName.Text,
                       txtDesignation.Text, txtAddress1.Text, City, DDLState.SelectedValue.ToString(), txtPin.Text,
                       DDLCountry.SelectedValue.ToString(), "", organizationType, "", "", "", "",
                        //txtVAT.Text, txtCST.Text, txtServiceTaxNo.Text, txtTIN.Text,              
                       ddlVendorCategory.SelectedItem.Text, dtContact, txtTANNo.Text.Trim(), Session["Employee_Code"].ToString());


                    if (result == "0" || result == null)
                    {
                        
                        Response.Write("<script language='javascript'>alert('Vendor Not Created');</script>");
                        string jScript;
                        //jScript = "<script type=text/javascript>javascript:window.parent.location.href='http://www.sgbiz.in/sggroupv2.2/HRM/DashBoard/FinanceMenu.aspx' ;</script>";
                        //Modified By Pramesh Kumar Vishwakarma
                        jScript = "<script type=text/javascript>javascript:window.parent.location.href='http://www.sgbiz.in/sggroupv2.2/HRM/DashBoard/FinanceMenu.aspx?header=M00023&Menu=M00034&c=" + lblCom.Text + "&b=" + lblBranch.Text + "&d=" + lblTdate.Text + "&cId=" + hiddcomp.Value + "&bId=" + hiddbranch.Value + "' ;</script>";
                        //jScript = "<script type=text/javascript>javascript:window.parent.location.href='http://localhost:51435/ERP_V01.2/HRM/DashBoard/FinanceMenu.aspx?header=M00023&Menu=M00034&c=" + lblCom.Text + "&b=" + lblBranch.Text + "&d=" + lblTdate.Text + "&cId=" + hiddcomp.Value + "&bId=" + hiddbranch.Value + "' ;</script>";
                        ClientScript.RegisterStartupScript(GetType(), "Javascript", jScript);
                    }
                     if (result == "1")
                     {
                        
                        Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());
                        Response.Write("<script language='javascript'>alert('Vendor Created Successfully');</script>");
                        string jScript;
                        // jScript = "<script type=text/javascript>javascript:window.parent.location.href='http://www.sgbiz.in/sggroupv2.2/HRM/DashBoard/FinanceMenu.aspx' ;</script>";
                        jScript = "<script type=text/javascript>javascript:window.parent.location.href='http://www.sgbiz.in/sggroupv2.2/HRM/DashBoard/FinanceMenu.aspx?header=M00023&Menu=M00034&c=" + lblCom.Text + "&b=" + lblBranch.Text + "&d=" + lblTdate.Text + "&cId=" + hiddcomp.Value + "&bId=" + hiddbranch.Value + "' ;</script>";
                        //jScript = "<script type=text/javascript>javascript:window.parent.location.href='http://localhost:51435/ERP_V01.2/HRM/DashBoard/FinanceMenu.aspx?header=M00023&Menu=M00034&c=" + lblCom.Text + "&b=" + lblBranch.Text + "&d=" + lblTdate.Text + "&cId=" + hiddcomp.Value + "&bId=" + hiddbranch.Value + "' ;</script>";

                        ClientScript.RegisterStartupScript(GetType(), "Javascript", jScript);
                        txtAddress1.Text = "";
                    }
                     if (result == "-2")
                     {

                             Response.Write("<script language='javascript'>alert('Enter atleast one Default Address');</script>");
                             string jScript;
                             jScript = "<script type=text/javascript>javascript:window.parent.location.href='http://www.sgbiz.in/sggroupv2.2/HRM/DashBoard/FinanceMenu.aspx?header=M00023&Menu=M00034&c=" + lblCom.Text + "&b=" + lblBranch.Text + "&d=" + lblTdate.Text + "&cId=" + hiddcomp.Value + "&bId=" + hiddbranch.Value + "' ;</script>";
                         
                    }
                }

            }
        }
        else
        {
            Response.Write("<script language='javascript'>alert('Enter atleast one Default Address.');</script>");
        }
    }

    protected void cbsBtn_Click(object sender, EventArgs e)
    {
        double result = 0;
        if (Session["update"].ToString() == ViewState["update"].ToString())
        {
            DataTable dtBank = new DataTable();
            dtBank.Columns.Add("bnkAccNumber", typeof(string));
            dtBank.Columns.Add("Closing", typeof(double));
            DataRow dr = null;
            foreach (GridViewRow gr in GVBankDetails.Rows)
            {
                dr = dtBank.NewRow();
                Label lblBankCode = (Label)gr.FindControl("lblBankCode");
                Label lblClosingAmt = (Label)gr.FindControl("lblClosingAmt");
                dr["bnkAccNumber"] = lblBankCode.Text;
                dr["Closing"] = Convert.ToDouble(lblClosingAmt.Text);
                dtBank.Rows.Add(dr);
            }

            result = CBS.Insert_CBSClosing(hiddcomp.Value, hiddbranch.Value, lblCBSDate.Text, Session["Employee_Code"].ToString(), txtCBSRemarks.Text, Convert.ToDouble(lblCashClosing.Text), Convert.ToDouble(dtBank.Compute("sum(Closing)", "")), dtBank, lblCBSDate.Text, lblCBSDate.Text);

            Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());
        }

        if (result == 0 || result == null)
        {
            Response.Write("<script language='javascript'>alert('CBS Locking Details Are not Inserted Successfully');</script>");
            string jScript;
            //jScript = "<script type=text/javascript>javascript:window.parent.location.href='http://www.sgbiz.in/sggroupv2.2/HRM/DashBoard/FinanceMenu.aspx' ;</script>";
            jScript = "<script type=text/javascript>javascript:window.parent.location.href='http://www.sgbiz.in/sggroupv2.2/HRM/DashBoard/FinanceMenu.aspx?header=M00023&Menu=M00034&c=" + lblCom.Text + "&b=" + lblBranch.Text + "&d=" + lblTdate.Text + "&cId=" + hiddcomp.Value + "&bId=" + hiddbranch.Value + "' ;</script>";
            ClientScript.RegisterStartupScript(GetType(), "Javascript", jScript);
        }
        else
        {
            string message = "CBS Locking Details Are Inserted Successfully.";
            string url = "http://www.sgbiz.in/sggroupv2.2/HRM/DashBoard/FinanceCalender.aspx?header=M00023&Menu=M00034";
            string script = "window.onload = function(){ alert('";
            script += message;
            script += "');";
            script += "window.location = '";
            script += url;
            script += "'; }";
            ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);

        }
    }


    public class MenuInfo
    {
        public string MenuId { get; set; }
        public int MLevel { get; set; }
        public string MenuText { get; set; }
        public string Murl { get; set; }
        public string ToolTips { get; set; }
        public string Description { get; set; }
        public string PMenuId { get; set; }
        public string MenuDept { get; set; }
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string CreateGenericArray(string Menu, string Empid, string Dept, string NodeType, string Date, string ccode, string bcode)
    {
        BAL_EmployeeDefaultAccess fin = new BAL_EmployeeDefaultAccess();
        BAL_EmployeeLevelAccess access = new BAL_EmployeeLevelAccess();
        List<MenuInfo> _MenuInfo = new List<MenuInfo>();
        BAL_FA_CommonMaster common = new BAL_FA_CommonMaster();
        string strCBSMenu = common.FA_SelectCBLMenu(Date, ccode, bcode);
        if ((Menu == "M00653" || Menu == "M00693" || Menu == "M00654" || Menu == "M00655") && NodeType == "P")
        {
            DataTable dt = access.LoadEmployeeDepartmentAccess(Empid);
            DataView dv = dt.DefaultView;
            dv.RowFilter = "[Department_Code] in ('DEPT000001','DEPT000002','DEPT000003','DEPT000004','DEPT000005','DEPT000006','DEPT000007','DEPT000008')";// and MenuID not in(" + strCBSMenu + "), Modified by Pramesh Kumar Vishwakarma
            dt = dv.ToTable();


            foreach (DataRow dr in dt.Rows)
            {
                MenuInfo lp = new MenuInfo();
                lp.MenuId = dr["Department_code"].ToString();
                lp.MLevel = 6;
                lp.MenuText = dr["Long_Title"].ToString();
                lp.Murl = "#";
                lp.ToolTips = dr["Short_Title"].ToString();
                lp.Description = dr["Short_Title"].ToString();
                lp.PMenuId = Menu;
                lp.MenuDept = dr["Department_code"].ToString();
                _MenuInfo.Add(lp);
            }
        }
        else
        {
            DataTable dt = fin.FinanceDashBoard(Empid, Menu);
            DataView dv1 = dt.DefaultView;
            dv1.RowFilter = " MenuID not in(" + strCBSMenu + ")";
            dt = dv1.ToTable();
            if (Dept != "")
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = "Dept='" + Dept + "'";
                dt = dv.ToTable();
            }
            foreach (DataRow dr in dt.Rows)
            {
                MenuInfo lp = new MenuInfo();
                lp.MenuId = dr["menuId"].ToString();
                lp.MLevel = int.Parse(dr["menuLevel"].ToString());
                lp.MenuText = dr["menuText"].ToString();
                lp.Murl = dr["url"].ToString();
                lp.ToolTips = dr["toolTips"].ToString();
                lp.Description = dr["Descr"].ToString();
                lp.PMenuId = dr["pMenuId"].ToString();
                lp.MenuDept = dr["Dept"].ToString();
                _MenuInfo.Add(lp);
            }
        }
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(_MenuInfo);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {       

        DataTable dtContact = (DataTable)ViewState["dtContact"];
        hdnVendorDiv.Value = "1";
        string GSTNO = txtGSTStatePan.Text.Trim().ToUpper() + txtGSTDigit.Text.Trim().ToUpper() + txtGSTConst.Text.Trim().ToUpper() + txtGSTCheckSum.Text.Trim().ToUpper();
        DataRow[] drRqList = dtContact.Select("GSTNo='" + GSTNO+ "'");
        if (drRqList.Length > 0)
        {
            ErrorContainer.Visible = true;
             //lblGSTINExist.Visible = true;
            msg.ShowMessage("GSTIN already added in list", null, ErrorContainer,  MyMessage, "Warning");
            return;
        }
        else
        {
            //lblGSTINExist.Visible = false;
            ErrorContainer.Visible = false;
            if (dtContact.Rows.Count > 0)
            {
                if (chkDefaultAddress.Checked == true)
                {
                    foreach (DataRow r in dtContact.Rows)
                    {
                        r["DefaultAddress"] = "0";
                    }
                }
            }

            DataRow dr = dtContact.NewRow();
            if (dtContact.Rows.Count > 0)
                dr["SR"] = Convert.ToInt32(dtContact.Rows[dtContact.Rows.Count - 1]["SR"].ToString()) + 1;
            else
                dr["SR"] = 1;

            dr["AddressType"] = ddlAddressType.SelectedValue.Trim();
            dr["AddressType_Name"] = ddlAddressType.SelectedItem.Text.Trim();
            if (chkDefaultAddress.Checked == true)
            {
                dr["DefaultAddress"] = 1;
            }
            else
            {
                dr["DefaultAddress"] = 0;
            }

            dr["Address1"] = txtAddress1.Text.Trim();

            dr["Country"] = DDLCountry.SelectedValue.Trim();
            dr["State"] = DDLState.SelectedValue.Trim();
            dr["StateName"] = DDLState.SelectedItem.Text.Trim();
            dr["City"] = DDLCity.SelectedValue.Trim();

            if (txtGSTConst.Text.Trim() != "" && txtGSTCheckSum.Text.Trim() != "" && txtGSTStatePan.Text.Trim() != "" && txtGSTDigit.Text.Trim() != "")
            {
                dr["GSTNo"] = (txtGSTStatePan.Text.Trim().ToUpper() + txtGSTDigit.Text.Trim().ToUpper() + txtGSTConst.Text.Trim().ToUpper() + txtGSTCheckSum.Text.Trim().ToUpper());
            }
            else
            {
                dr["GSTNo"] = "";
            }
           
            dr["Pin"] = txtPin.Text.Trim();
            dr["PhoneNo_GST"] = txtVendorPhone.Text.Trim();
            dr["FaxNo_GST"] = txtVendorFax.Text.Trim();
            dr["EmailId"] = txtVendorEmail.Text.Trim();
            dr["WebSite"] = txtVendorWebsite.Text.Trim();
            dr["PAN"] = txtPanNo.Text.Trim().ToUpper();
            dr["TAN"] = txtTANNo.Text.Trim().ToUpper();
            //----------- Contact Details -------------------------------------------------
            dr["Person"] = txtName.Text.Trim();
            dr["Designation"] = txtDesignation.Text.Trim();
            dr["Department"] = txtDepartment.Text.Trim();
            dr["Email"] = txtContactEmail.Text.Trim();
            dr["PhoneNo"] = txtContactPhone.Text.Trim();
            dr["FaxNo"] = txtContactFax.Text.Trim();
            dr["Mobile"] = txtContactMobile.Text.Trim();
            dr["Status"] = "I";

            dtContact.Rows.Add(dr);
            ViewState["dtContact"] = dtContact;

            GVContact.DataSource = dtContact;
            GVContact.DataBind();

            hdnVendorDiv.Value = "1";

            if (GVContact.Rows.Count > 0)
            {
                btnSubmit.Visible = true;
            }
            else
            {
                btnSubmit.Visible = false;
            }

            txtVendorName.Enabled = false;
            DDLIndustry.Enabled = false;
            DDLBranch.Enabled = false;
            ddlOrganizationType.Enabled = false;
            ddlVendorCategory.Enabled = false;
            DDLVendorType.Enabled = false;

            //txtPanNo.Enabled = false;
            if (txtTANNo.Text.Trim() != string.Empty)
            {
                //txtTANNo.Enabled = false;
                txtTANNo.Attributes.Add("readonly", "readonly");
            }

            txtPanNo.Attributes.Add("readonly", "readonly");
            Reset();
        }
    }


    public void Reset()
    {
        ddlAddressType.SelectedValue = "0";
        chkDefaultAddress.Checked = false;
        txtAddress1.Text = "";
        txtVendorEmail.Text = "";
        txtVendorFax.Text = "";
        txtVendorPhone.Text = "";
        txtVendorWebsite.Text = "";
        txtGSTCheckSum.Text = "";
        txtGSTDigit.Text = "";     
        txtPin.Text = "";
        txtName.Text = "";
        txtDepartment.Text = "";
        txtDesignation.Text = "";
        txtContactEmail.Text = "";
        txtContactFax.Text = "";
        txtContactMobile.Text = "";
        txtContactPhone.Text = "";
        txtCity.Text = "";
        CascadingDropDown_State.SelectedValue = "0";
        CascadingDropDown_City.SelectedValue = "0";      
        
    }
    protected void GVContact_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            hdnVendorDiv.Value = "1";

            int row = e.RowIndex;
            Label lblSR = (Label)GVContact.Rows[row].FindControl("lblSR");
            if (lblSR != null)
            {
                DataTable dtContact = (DataTable)ViewState["dtContact"];
                DataRow dr1 = dtContact.Rows.Find(lblSR.Text);
                if (dr1 != null)
                {
                    dtContact.Rows.Remove(dr1);
                    GVContact.DataSource = dtContact;
                    GVContact.DataBind();
                    ViewState["dtContact"] = dtContact;
                }
            }
            if (GVContact.Rows.Count > 0)
            {
                btnSubmit.Visible = true;
            }
            else
            {
                btnSubmit.Visible = false;
            }
        }
        catch (Exception exp)
        {            
          // MyMessage.Text = "Error : " + exp;
        }
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<string> Check_GST_No(string gstNo)
    {
        BALSGX_VendorMaster VendorGST = new BALSGX_VendorMaster();
        DataTable dt = VendorGST.CheckDuplicateVendor_GSTIN(gstNo);
        List<string> details = new List<string>();
        details.Clear();
        string user = string.Empty;
        if (dt.Rows.Count > 0)
        {
            user = Convert.ToString(dt.Rows[0]["GSTIN"]);
            details.Add(user);
        }
        return details;
    }

}
