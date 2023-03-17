using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace MIDPROJ
{
    public partial class Form1 : MaterialForm

    {
        public Int32 Rl;
        public Int32 assess;
        public Int32 Rubric;
        public Int32 Clos;
        public Int32 active;
        public Int32 Inactive;
        //Int32 Ids1;
        public int status;
        Int32 Ids=0;
        int assesscomponentdelid;
        int assessamentdelid;
        public int rid = 1;
        public int cid;
        public int sid;
        public int delrid;
        public int delrlid;
        readonly MaterialSkin.MaterialSkinManager materialSkinManager;
        public Form1()
        {

            InitializeComponent();
            materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.Teal500, MaterialSkin.Primary.Teal700, MaterialSkin.Primary.Teal100, MaterialSkin.Accent.Pink100, MaterialSkin.TextShade.WHITE);

            //errors
            //FirstNameErrorProvider = new ErrorProvider();
            //FirstNameErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
        }
        public void rubricidgetter()
        {
            try
            {
                var con1 = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("SELECT MAX(Id) FROM Rubric", con1);
                object result = cmd1.ExecuteScalar();
                if (result == DBNull.Value || result == null)
                {
                    Ids = 1;
                }
                else
                {
                    Ids = Convert.ToInt32(result) + 1;
                }

            }
            catch (Exception exp)
            {
                MaterialMessageBox.Show(exp.Message);
            }
            textBox6.Text = Ids.ToString();
        }
        public void loadinActive()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id,RegistrationNumber,FirstName,LastName,Contact,Email from Student where status=6", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView14.DataSource = dt;

        }
        public void clearEval()
        {
            textBox16.Clear();
            comboBox9.Text = "";
            comboBox10.Text = "";
        }
        public void clearAssesscoptexts()
        {
            textBox14.Clear();
            textBox15.Clear();
            textBox17.Clear();
            comboBox8.Text = "";
            comboBox7.Text = "";
        }
        public void clearAssestexts()
        {
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
        }
        public void clearrubricleveltexts()
        {
            comboBox2.Text = "";
            comboBox3.Text = "";
            textBox5.Clear();
            richTextBox2.ResetText();
        }
        public void clearrubrictexts()
        {
            comboBox1.Text = "";
            textBox6.Clear();
            richTextBox1.ResetText();
        }
        public void clearclotexts()
        {
            textBox3.Clear();
            textBox4.Clear();
        }
        public void clearsttexts()
        {
            txtFname.Clear();
            txtlname.Clear();
            textBox1.Clear();
            textBox2.Clear();
            txtaddress.Clear();


        }
        public void LoadEvaluation()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select Id,FirstName+' '+LastName as Name,RegistrationNumber,Email from Student where status='5'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView8.DataSource = dt;
        }
        public void RubricLevel()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select count(*) from Rubric", con);
            Rl = (Int32)cmd.ExecuteScalar();
            materialLabel6.Text = "Rubric Levels: " + Rl.ToString();

        }
        public void Assessments()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select count(*) from Rubric", con);
            assess = (Int32)cmd.ExecuteScalar();
            materialLabel5.Text = "Assessments: " + assess.ToString();

        }
        public void Rubrics()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select count(*) from Rubric", con);
            Rubric = (Int32)cmd.ExecuteScalar();
            materialLabel4.Text = "Rubrics: " + Rubric.ToString();

        }
        public void CLO()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select count(*) from Clo", con);
            Clos = (Int32)cmd.ExecuteScalar();
            materialLabel3.Text = "CLO's: " + Clos.ToString();

        }
        public void loadActive()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select count(*) from Student where status='5'", con);
            active = (Int32)cmd.ExecuteScalar();
            materialLabel1.Text = "Active Students: " + active.ToString();

        }
        public void loadInActive()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select count(*) from Student where status='6'", con);
            Inactive = (Int32)cmd.ExecuteScalar();
            materialLabel2.Text = "InActive Students: " + Inactive.ToString();

        }
        public bool ValidateInput(int input)
        {
            if (input >= 1 && input <= 100)
            {
                return true;
            }
            else
            {
                MaterialMessageBox.Show("Input value must be between 1 and 100.");
                return false;
            }
        }

        private bool ValidateCLO(string cloNumber)
        {
            // Define a regular expression to match a valid CLO number format
            Regex regex = new Regex("^[a-zA-Z \t\r\n\f]+$");
            // Check if the input matches the regular expression
            if (!regex.IsMatch(cloNumber))
            {
                // Display an error message to the user
                MaterialMessageBox.Show("Please enter a valid CLO ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ValidateRegistrationNumber(string registrationNumber)
        {
            // Define a regular expression to match a valid registration number format
            Regex regex = new Regex(@"^(20[0-1][0-9]|202[0-3])-[A-Za-z]{2,}-\d{1,}$");

            // Check if the input matches the regular expression
            if (!regex.IsMatch(registrationNumber))
            {
                // Display an error message to the user
                MaterialMessageBox.Show("Please enter a valid registration number in the format of 2021-CS-13 or 2021-CS-123 and ensure the year is between 2000 and 2023.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ValidateEmail(string email)
        {
            // Define a regular expression to match a valid email address format
            Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            // Check if the input matches the regular expression
            if (!regex.IsMatch(email))
            {
                // Display an error message to the user
                MaterialMessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            // Define a regular expression to match a valid Pakistan phone number format with or without hyphen
            Regex regex = new Regex(@"^(03[0-9]{2}-?[0-9]{7})?$");

            // Check if the input matches the regular expression
            if (!regex.IsMatch(phoneNumber))
            {
                // Display an error message to the user
                MaterialMessageBox.Show("Please enter a valid phone number in the format of 03xx-xxxxxxx or 03xxxxxxxxx.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool ValidateAssessName(string firstName)
        {
            // Define a regular expression to match only letters
            Regex regex = new Regex("^[a-zA-Z\\s]+$");

            // Check if the input matches the regular expression
            if (!regex.IsMatch(firstName))
            {
                // Display an error message to the user
                MaterialMessageBox.Show("Please enter a valid  name. The name should contain only letters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }



        private bool ValidateFirstName(string firstName)
        {
            // Define a regular expression to match only letters
            Regex regex = new Regex("^[a-zA-Z \t\r\n\f]+$");
            // Check if the input matches the regular expression
            if (!regex.IsMatch(firstName))
            {
                // Display an error message to the user
                MaterialMessageBox.Show("Please enter a valid first name. The name should contain only letters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool ValidateLastName(string LastName)
        {
            // Define a regular expression to match only letters
            Regex regex = new Regex("^[a-zA-Z\\s]*$");
            // Check if the input matches the regular expression
            if (!regex.IsMatch(LastName))
            {
                // Display an error message to the user
                MaterialMessageBox.Show("Please enter a valid Last name. The name should contain only letters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadinActive();
            //txtFname.TextChanged += new EventHandler(txtFname_TextChanged);
            LoadEvaluation();
            RubricLevel();
            Assessments();
            Rubrics();
            CLO();
            loadActive();
            loadInActive();
            Thread.BeginThreadAffinity();
            attendance();
            // Create first checkbox column
            //DataGridViewCheckBoxColumn checkBoxColumn1 = new DataGridViewCheckBoxColumn();
            //checkBoxColumn1.HeaderText = "Present";
            //checkBoxColumn1.Width = 50;
            //checkBoxColumn1.Name = "checkBoxColumn1";
            //checkBoxColumn1.TrueValue = "Yes";
            //checkBoxColumn1.FalseValue = "No";

            //// Create second checkbox column
            //DataGridViewCheckBoxColumn checkBoxColumn2 = new DataGridViewCheckBoxColumn();
            //checkBoxColumn2.HeaderText = "Absent";
            //checkBoxColumn2.Width = 50;
            //checkBoxColumn2.Name = "checkBoxColumn2";
            //checkBoxColumn2.TrueValue = "On";
            //checkBoxColumn2.FalseValue = "Off";

            //// Add columns to DataGridView control
            //DataGridViewCheckBoxColumn checkBoxColumn3 = new DataGridViewCheckBoxColumn();
            //checkBoxColumn3.HeaderText = "Leave";
            //checkBoxColumn3.Width = 50;
            //checkBoxColumn3.Name = "checkBoxColumn3";
            //checkBoxColumn3.TrueValue = "On";
            //checkBoxColumn3.FalseValue = "Off";

            //DataGridViewCheckBoxColumn checkBoxColumn4 = new DataGridViewCheckBoxColumn();
            //checkBoxColumn4.HeaderText = "Late";
            //checkBoxColumn4.Width = 50;
            //checkBoxColumn4.Name = "checkBoxColumn4";
            //checkBoxColumn4.TrueValue = "On";
            //checkBoxColumn4.FalseValue = "Off";

            //dataGridView5.Columns.Add(checkBoxColumn1);
            //dataGridView5.Columns.Add(checkBoxColumn2);
            //dataGridView5.Columns.Add(checkBoxColumn3);
            //dataGridView5.Columns.Add(checkBoxColumn4);

            if (textBox7.Text == "")
            {
                textBox7.Text = "Enter Search text here";
                textBox7.ForeColor = SystemColors.GrayText;
            }
            if (textBox9.Text == "")
            {
                textBox9.Text = "Enter Search text here";
                textBox9.ForeColor = SystemColors.GrayText;
            }
            if (textBox8.Text == "")
            {
                textBox8.Text = "Enter name here to search";
                textBox8.ForeColor = SystemColors.GrayText;
            }


            //if(textBox5.Text=="" && richTextBox2.Text == "" && comboBox3.Text == "")
            //{
            //    textBox5.Text = 1.ToString();
            //}
            //else
            //{
            //    var con2 = Configuration.getInstance().getConnection();
            //    SqlCommand cmd2 = new SqlCommand("Select max(Id)+1 from RubricLevel", con2);
            //    Ids1 = (Int32)cmd2.ExecuteScalar();
            //    cmd2.ExecuteNonQuery();
            //    textBox5.Text = Ids.ToString();
            //}
            rubricidgetter();

            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Clo", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.ValueMember = "Id";
            comboBox1.DataSource = dt;

            var cons = Configuration.getInstance().getConnection();
            SqlCommand cmds = new SqlCommand("Select Id from Rubric", cons);
            SqlDataAdapter das = new SqlDataAdapter(cmds);
            DataTable dts = new DataTable();
            das.Fill(dts);
            comboBox2.ValueMember = "Id";
            comboBox2.DataSource = dts;

            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select Id from Rubric", con2);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            comboBox8.ValueMember = "Id";
            comboBox8.DataSource = dt2;

            var con3 = Configuration.getInstance().getConnection();
            SqlCommand cmd3 = new SqlCommand("Select Id from Assessment", con3);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            comboBox7.ValueMember = "Id";
            comboBox7.DataSource = dt3;
        }

        private void btncreate_Click(object sender, EventArgs e)
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select LookupId from Lookup where Name='Active'", con1);
            status = (Int32)cmd1.ExecuteScalar();
            cmd1.ExecuteNonQuery();


            if (txtFname.Text != "" && textBox2.Text != "" && txtaddress.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand check = new SqlCommand("select count(*) from student where RegistrationNumber='" + txtaddress.Text + "'", con);
                    SqlCommand check1 = new SqlCommand("select count(*) from student where Email='" + textBox2.Text + "'", con);
                    int count = Convert.ToInt32(check.ExecuteScalar());
                    int count1 = Convert.ToInt32(check1.ExecuteScalar());
                    if (count == 0 && count1 == 0)
                    {
                        SqlCommand cmd = new SqlCommand("Insert into Student values (@FirstName,@LastName,@Contact,@Email,@RegistrationNumber,@Status)", con);
                        if (ValidateFirstName(txtFname.Text) == true)
                        {
                            cmd.Parameters.AddWithValue("@FirstName", txtFname.Text);
                            if (ValidateLastName(txtlname.Text) == true)
                            {
                                cmd.Parameters.AddWithValue("@LastName", txtlname.Text);
                                if (ValidatePhoneNumber(textBox1.Text) == true)
                                {
                                    cmd.Parameters.AddWithValue("@Contact", textBox1.Text);
                                    if (ValidateEmail(textBox2.Text) == true)
                                    {
                                        cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                                        if (ValidateRegistrationNumber(txtaddress.Text) == true)
                                        {
                                            cmd.Parameters.AddWithValue("@RegistrationNumber", txtaddress.Text);
                                        }
                                    }
                                }
                            }
                        }
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.ExecuteNonQuery();
                        MaterialMessageBox.Show("Successfully saved", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clearsttexts();
                    }
                    else
                    {
                        MaterialMessageBox.Show("Registration Number or Email Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                MaterialMessageBox.Show("Input All Credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        public void studeninsert()
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select LookupId from Lookup where Name='Active'", con1);
            status = (Int32)cmd1.ExecuteScalar();
            cmd1.ExecuteNonQuery();


            if (txtFname.Text != "" && txtlname.Text != "" && textBox1.Text != "" && textBox2.Text != "" && txtaddress.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand check = new SqlCommand("select count(*) from student where RegistrationNumber='" + txtaddress.Text + "'", con);
                    SqlCommand check1 = new SqlCommand("select count(*) from student where Email='" + textBox2.Text + "'", con);
                    int count = Convert.ToInt32(check.ExecuteScalar());
                    int count1 = Convert.ToInt32(check1.ExecuteScalar());
                    if (count == 0 && count1 == 0)
                    {
                        SqlCommand cmd = new SqlCommand("Insert into Student values (@FirstName,@LastName,@Contact,@Email,@RegistrationNumber,@Status)", con);
                        if (ValidateFirstName(txtFname.Text) == true)
                        {
                            cmd.Parameters.AddWithValue("@FirstName", txtFname.Text.Trim());
                        }
                        if (ValidateLastName(txtlname.Text) == true)
                        {
                            cmd.Parameters.AddWithValue("@LastName", txtlname.Text);
                        }
                        if (ValidatePhoneNumber(textBox1.Text) == true)
                        {
                            cmd.Parameters.AddWithValue("@Contact", textBox1.Text);
                        }
                        if (ValidateEmail(textBox2.Text) == true)
                        {
                            cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                        }
                        if (ValidateRegistrationNumber(txtaddress.Text) == true)
                        {
                            cmd.Parameters.AddWithValue("@RegistrationNumber", txtaddress.Text);
                        }
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.ExecuteNonQuery();
                        MaterialMessageBox.Show("Successfully saved", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MaterialMessageBox.Show("Registration Number or Email Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                MaterialMessageBox.Show("Input All Credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnretrieve_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id,RegistrationNumber,FirstName,LastName,Contact,Email from Student where status=5", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            clearsttexts();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            txtaddress.Text = dataGridView1.Rows[e.RowIndex].Cells["RegistrationNumber"].Value.ToString();
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select Id from Student where RegistrationNumber=@RegistrationNumber", con);
            cmd.Parameters.AddWithValue("RegistrationNumber", txtaddress.Text);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            sid = id;
            txtFname.Text = dataGridView1.Rows[e.RowIndex].Cells["FirstName"].Value.ToString();
            txtlname.Text = dataGridView1.Rows[e.RowIndex].Cells["LastName"].Value.ToString();
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["Contact"].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();


        }

        private void btndel_Click(object sender, EventArgs e)
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select LookupId from Lookup where Name='InActive'", con1);
            status = (Int32)cmd1.ExecuteScalar();
            cmd1.ExecuteNonQuery();
            if (txtFname.Text != "" && textBox2.Text != "" && txtaddress.Text != "")
            {

                try
                {
                    var con = Configuration.getInstance().getConnection();
                    //string Id=Convert.ToString(ProcessCmdKey)
                    SqlCommand cmd = new SqlCommand("update Student set Status=@Status where Id=@Id", con);
                    //MaterialMessageBox.Show(sid.ToString());
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Id", sid);
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Successfully deleted", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearsttexts();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter a Unique Credential to Delete", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select LookupId from Lookup where Name='Active'", con1);
            status = (Int32)cmd1.ExecuteScalar();
            cmd1.ExecuteNonQuery();
            if (txtFname.Text != "" && textBox2.Text != "" && txtaddress.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand check = new SqlCommand("select count(*) from student where Id<>@Id and  RegistrationNumber='" + txtaddress.Text + "'", con);
                    check.Parameters.AddWithValue("@Id", sid);
                    SqlCommand check1 = new SqlCommand("select count(*) from student where  Id<>@Id and Email='" + textBox2.Text + "'", con);
                    check1.Parameters.AddWithValue("@Id", sid);
                    int count = Convert.ToInt32(check.ExecuteScalar());
                    int count1 = Convert.ToInt32(check1.ExecuteScalar());

                    if (count == 0 && count1 == 0)
                    {
                        SqlCommand cmd = new SqlCommand("Update Student set FirstName=@FirstName,LastName=@LastName,Contact=@Contact,Email=@Email,RegistrationNumber=@RegistrationNumber,status=5 where Id=@Id", con);
                        if (ValidateFirstName(txtFname.Text) == true)
                        {
                            cmd.Parameters.AddWithValue("@FirstName", txtFname.Text.Trim());
                            if (ValidateLastName(txtlname.Text) == true)
                            {
                                cmd.Parameters.AddWithValue("@LastName", txtlname.Text);
                                if (ValidatePhoneNumber(textBox1.Text) == true)
                                {
                                    cmd.Parameters.AddWithValue("@Contact", textBox1.Text);
                                    if (ValidateEmail(textBox2.Text) == true)
                                    {
                                        cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                                        if (ValidateRegistrationNumber(txtaddress.Text) == true)
                                        {
                                            cmd.Parameters.AddWithValue("@RegistrationNumber", txtaddress.Text);
                                        }
                                    }
                                }
                            }
                        }
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@Id", sid);
                        cmd.ExecuteNonQuery();
                        MaterialMessageBox.Show("Successfully updated", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clearsttexts();
                    }
                    else
                    {
                        MaterialMessageBox.Show("Registration Number or Email Already Exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand check1 = new SqlCommand("select count(*) from Clo where Name='" + textBox4.Text + "'", con);
                    int count = Convert.ToInt32(check1.ExecuteScalar());
                    if (count == 0)
                    {
                        SqlCommand cmd = new SqlCommand("Insert into Clo values (@Name,@DateCreated,@DateUpdated)", con);
                        if (ValidateCLO(textBox4.Text) == true)
                        {
                            cmd.Parameters.AddWithValue("@Name", textBox4.Text);
                        }
                        DateTime date = DateTime.Now;
                        cmd.Parameters.AddWithValue("@DateCreated", date);
                        cmd.Parameters.AddWithValue("@DateUpdated", date);
                        cmd.ExecuteNonQuery();
                        MaterialMessageBox.Show("Successfully saved", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clearclotexts();
                    }
                    else
                    {
                        MaterialMessageBox.Show("CLO name already exists", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception)
                {
                }

            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
            textBox3.Text = dataGridView2.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            textBox4.Text = dataGridView2.Rows[e.RowIndex].Cells["Name"].Value.ToString();
            //dateTimePicker1.Text = dataGridView2.Rows[e.RowIndex].Cells["DateCreated"].Value.ToString();
            cid = int.Parse(textBox3.Text.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Clo", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            clearclotexts();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "" && textBox3.Text != "")
            {
                try
                {
                    //var con = Configuration.getInstance().getConnection();
                    cid = int.Parse(textBox3.Text.ToString());
                    //SqlCommand cmd = new SqlCommand("Delete from Clo where Id=@Id", con);
                    //cmd.Parameters.AddWithValue("@Id", cid);
                    //cmd.ExecuteNonQuery();
                    //MaterialMessageBox.Show("Successfully deleted");
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand temp3 = new SqlCommand("Delete FROM StudentResult WHERE AssessmentComponentId In (Select Id From AssessmentComponent WHERE RubricId In (Select Id From Rubric Where CloId = @Id) )", con);
                    temp3.Parameters.AddWithValue("@Id", cid);
                    temp3.ExecuteNonQuery();
                    SqlCommand temp2 = new SqlCommand("Delete FROM AssessmentComponent WHERE RubricId In (Select Id From Rubric Where CloId = @Id)", con);
                    temp2.Parameters.AddWithValue("@Id", cid);
                    temp2.ExecuteNonQuery();
                    SqlCommand temp1 = new SqlCommand("Delete FROM RubricLevel WHERE RubricId In  (Select Id From Rubric Where CloId = @Id)", con);
                    temp1.Parameters.AddWithValue("@Id", cid);
                    temp1.ExecuteNonQuery();
                    SqlCommand temp = new SqlCommand("Delete FROM Rubric WHERE CloId = @Id", con);
                    temp.Parameters.AddWithValue("@Id", cid);
                    temp.ExecuteNonQuery();
                    SqlCommand cmd = new SqlCommand("Delete FROM Clo WHERE Id= @Id", con);
                    cmd.Parameters.AddWithValue("@Id", cid);
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Successfully Deleted", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearclotexts();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter a Unique Credential to delete", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    MaterialMessageBox.Show(cid.ToString());
                    SqlCommand cmd = new SqlCommand("UPDATE Clo set Name=@Name,DateUpdated=@DateUpdated where Id=@Id", con);
                    if (ValidateCLO(textBox4.Text) == true)
                    {
                        cmd.Parameters.AddWithValue("@Name", textBox4.Text);
                    }
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Id", cid);
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Successfully Updated", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearclotexts();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select * from Clo", con1);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && comboBox1.Text != "" && richTextBox1.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Insert into Rubric values (@Id,@Details,@CloId)", con);
                    cmd.Parameters.AddWithValue("Id", textBox6.Text);
                    cmd.Parameters.AddWithValue("CloId", comboBox1.Text);
                    cmd.Parameters.AddWithValue("Details", richTextBox1.Text);
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Successfully Saved", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearrubrictexts();
                    rid++;
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select * from Rubric", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView3.DataSource = dt;
                var con1 = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("Select max(Id)+1 from Rubric", con1);
                Ids = (Int32)cmd1.ExecuteScalar();
                cmd1.ExecuteNonQuery();
                textBox6.Text = Ids.ToString();
                clearrubrictexts();
            }
            catch (Exception)
            {
                //MaterialMessageBox.Show(exp.Message);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && comboBox1.Text != "" && richTextBox1.Text != "")
            {
                try
                {
                    delrid = int.Parse(textBox6.Text);
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd1 = new SqlCommand("DELETE FROM StudentResult WHERE AssessmentComponentId IN (SELECT Id FROM AssessmentComponent WHERE RubricId = @RubricId)", con);
                    cmd1.Parameters.AddWithValue("@RubricId", delrid);
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("DELETE FROM AssessmentComponent WHERE RubricId = @RubricId", con);
                    cmd2.Parameters.AddWithValue("@RubricId", delrid);
                    cmd2.ExecuteNonQuery();

                    SqlCommand cmd3 = new SqlCommand("DELETE FROM RubricLevel WHERE RubricId = @RubricId", con);
                    cmd3.Parameters.AddWithValue("@RubricId", delrid);
                    cmd3.ExecuteNonQuery();

                    SqlCommand cmd4 = new SqlCommand("DELETE FROM Rubric WHERE Id = @RubricId", con);
                    cmd4.Parameters.AddWithValue("@RubricId", delrid);
                    cmd4.ExecuteNonQuery();

                    MaterialMessageBox.Show("Successfully Deleted", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearrubrictexts();


                }
                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter A unique Credential to Delete", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView3.CurrentRow.Selected = true;
                comboBox1.Text = dataGridView3.Rows[e.RowIndex].Cells["CloId"].Value.ToString();
                textBox6.Text = dataGridView3.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                richTextBox1.Text = dataGridView3.Rows[e.RowIndex].Cells["Details"].Value.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && comboBox1.Text != "" && richTextBox1.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("update Rubric set Details=@Details,CloId=@CloId where Id=@Id", con);
                    cmd.Parameters.AddWithValue("Details", richTextBox1.Text);
                    cmd.Parameters.AddWithValue("CloId", comboBox1.Text);
                    //MaterialMessageBox.Show(Ids.ToString());
                    cmd.Parameters.AddWithValue("Id", textBox6.Text);
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Successfully Updated", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearrubrictexts();
                }
                catch (Exception)
                {
                }
            }
            MaterialMessageBox.Show("Enter all Credentials", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }



        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "" && richTextBox2.Text != "" && comboBox3.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();

                    // Check if a RubricLevel with the same RubricId and Details already exists
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM RubricLevel WHERE RubricId = @RubricId AND Details = @Details", con);
                    checkCmd.Parameters.AddWithValue("@RubricId", int.Parse(comboBox2.Text));
                    checkCmd.Parameters.AddWithValue("@Details", richTextBox2.Text);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        // If the RubricLevel does not already exist, insert it into the database
                        SqlCommand cmd = new SqlCommand("Insert into RubricLevel values(@RubricId,@Details,@MeasurementLevel)", con);
                        cmd.Parameters.AddWithValue("@RubricId", int.Parse(comboBox2.Text));
                        cmd.Parameters.AddWithValue("@Details", richTextBox2.Text);
                        cmd.Parameters.AddWithValue("@MeasurementLevel", int.Parse(comboBox3.Text));
                        cmd.ExecuteNonQuery();
                        MaterialMessageBox.Show("Successfully Saved", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clearrubricleveltexts();
                    }
                    else
                    {
                        // If the RubricLevel already exists, show an error message
                        MaterialMessageBox.Show("A RubricLevel with the same RubricId and Details already exists", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during the database operation
                    MaterialMessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



        }

        private void button10_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from RubricLevel", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView4.DataSource = dt;
            clearrubricleveltexts();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "" && comboBox3.Text != "" && richTextBox2.Text != "" && textBox5.Text != "")
            {
                try
                {
                    delrlid = int.Parse(textBox5.Text);

                    var con = Configuration.getInstance().getConnection();

                    SqlCommand cmd1 = new SqlCommand("DELETE FROM StudentResult WHERE AssessmentComponentId IN (SELECT Id FROM AssessmentComponent WHERE RubricId IN (SELECT RubricId FROM RubricLevel WHERE Id = @Id))", con);
                    cmd1.Parameters.AddWithValue("@Id", delrlid);
                    cmd1.ExecuteNonQuery();

                    //SqlCommand cmd2 = new SqlCommand("DELETE FROM AssessmentComponent WHERE RubricId IN (SELECT RubricId FROM RubricLevel WHERE Id = @Id)", con);
                    //cmd2.Parameters.AddWithValue("@Id", delrlid);
                    //cmd2.ExecuteNonQuery();


                    SqlCommand cmd3 = new SqlCommand("DELETE FROM RubricLevel WHERE Id = @Id", con);
                    cmd3.Parameters.AddWithValue("@Id", delrlid);
                    cmd3.ExecuteNonQuery();

                    MaterialMessageBox.Show("Successfully Deleted", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearrubricleveltexts();

                }
                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter a Unique Credential to Delete", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //if (comboBox2.Text != "" && richTextBox2.Text != "" && comboBox3.Text != "")
            //{
            //    try
            //    {
            //        var con = Configuration.getInstance().getConnection();
            //        SqlCommand cmd = new SqlCommand("Update  RubricLevel set (RubricId=@RubricId,Details=@Details,MeasurementLevel=@MeasurementLevel) where Id=@Id", con);
            //        cmd.Parameters.AddWithValue("@RubricId", int.Parse(comboBox2.Text));
            //        cmd.Parameters.AddWithValue("@Details", richTextBox2.Text);
            //        cmd.Parameters.AddWithValue("@MeasurementLevel", int.Parse(comboBox3.Text));
            //        cmd.ExecuteNonQuery();
            //        MaterialMessageBox.Show("Successfully Updated", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        clearrubricleveltexts();

            //    }
            //    catch (Exception)
            //    {
            //    }
            //}
            if (comboBox2.Text != "" && richTextBox2.Text != "" && comboBox3.Text != "")
            {

                var con = Configuration.getInstance().getConnection();

                // Check if a RubricLevel with the same RubricId and Details already exists
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM RubricLevel WHERE RubricId = @RubricId AND Details = @Details and Id<>@Id", con);
                checkCmd.Parameters.AddWithValue("@RubricId", int.Parse(comboBox2.Text));
                checkCmd.Parameters.AddWithValue("@Details", richTextBox2.Text);
                checkCmd.Parameters.AddWithValue("@Id", int.Parse(textBox5.Text));
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    SqlCommand cmd = new SqlCommand("Update  RubricLevel set RubricId=@RubricId,Details=@Details,MeasurementLevel=@MeasurementLevel where Id=@Id", con);
                    cmd.Parameters.AddWithValue("@RubricId", int.Parse(comboBox2.Text));
                    cmd.Parameters.AddWithValue("@Details", richTextBox2.Text);
                    cmd.Parameters.AddWithValue("@MeasurementLevel", int.Parse(comboBox3.Text));
                    cmd.Parameters.AddWithValue("@Id", int.Parse(textBox5.Text));
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Successfully Updated", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearrubricleveltexts();
                }
                else
                {
                    // If the RubricLevel already exists, show an error message
                    MaterialMessageBox.Show("A RubricLevel with the same RubricId and Details already exists", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView4.CurrentRow.Selected = true;
                comboBox2.Text = dataGridView4.Rows[e.RowIndex].Cells["RubricId"].Value.ToString();
                comboBox3.Text = dataGridView4.Rows[e.RowIndex].Cells["MeasurementLevel"].Value.ToString();
                textBox5.Text = dataGridView4.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                richTextBox2.Text = dataGridView4.Rows[e.RowIndex].Cells["Details"].Value.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void textBox7_MouseHover(object sender, EventArgs e)
        {
            //if (textBox7.Text == "Enter text here")
            //{
            //    textBox7.Text = "";
            //    textBox7.ForeColor = SystemColors.WindowText;
            //}
        }

        private void textBox7_MouseLeave(object sender, EventArgs e)
        {
            //if (textBox7.Text == "")
            //{
            //    textBox7.Text = "Enter text here";
            //    textBox7.ForeColor = SystemColors.GrayText;
            //}
        }

        private void textBox7_Click(object sender, EventArgs e)
        {

            if (textBox7.Text == "Enter Search text here")
            {
                textBox7.Text = "";
                textBox7.ForeColor = SystemColors.WindowText;
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.CurrentRow.Selected = true;
                txtaddress.Text = dataGridView1.Rows[e.RowIndex].Cells["RegistrationNumber"].Value.ToString();
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("select Id from Student where RegistrationNumber=@RegistrationNumber", con);
                cmd.Parameters.AddWithValue("RegistrationNumber", txtaddress.Text);
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                sid = id;
                txtFname.Text = dataGridView1.Rows[e.RowIndex].Cells["FirstName"].Value.ToString();
                txtlname.Text = dataGridView1.Rows[e.RowIndex].Cells["LastName"].Value.ToString();
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["Contact"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            var con = Configuration.getInstance().getConnection();
            string sortColumn = comboBox4.SelectedItem.ToString();
            if (sortColumn != "")
            {
                string sqlQuery = "Select Id,RegistrationNumber,FirstName,LastName,Contact,Email from Student where Status=5 ORDER BY " + sortColumn;
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            //string search = comboBox5.Text;
            //string a = search.ToString();
            string texts = textBox7.Text;
            if (comboBox5.Text != "")
            {
                if (comboBox5.Text == "FirstName")
                {
                    SqlCommand cmd = new SqlCommand("Select Id, FirstName, LastName, Contact, Email, RegistrationNumber from Student Where FirstName" + " LIKE '%" + texts + "%'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                if (comboBox5.Text == "LastName")
                {
                    SqlCommand cmd = new SqlCommand("Select Id, FirstName, LastName, Contact, Email, RegistrationNumber from Student Where LastName" + " LIKE '%" + texts + "%'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                if (comboBox5.Text == "Contact")
                {
                    SqlCommand cmd = new SqlCommand("Select Id, FirstName, LastName, Contact, Email, RegistrationNumber from Student Where Contact" + " LIKE '%" + texts + "%'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                if (comboBox5.Text == "Email")
                {
                    SqlCommand cmd = new SqlCommand("Select Id, FirstName, LastName, Contact, Email, RegistrationNumber from Student Where Email" + " LIKE '%" + texts + "%'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                if (comboBox5.Text == "RegistrationNumber")
                {
                    SqlCommand cmd = new SqlCommand("Select Id, FirstName, LastName, Contact, Email, RegistrationNumber from Student Where RegistrationNumber" + " LIKE '%" + texts + "%'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                if (comboBox5.Text == "Id")
                {
                    SqlCommand cmd = new SqlCommand("Select Id, FirstName, LastName, Contact, Email, RegistrationNumber from Student Where Id" + " LIKE '%" + texts + "%'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }

            }
        }

        private void tableLayoutPanel17_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {

            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Clo", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.ValueMember = "Id";
            comboBox1.DataSource = dt;
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            var cons = Configuration.getInstance().getConnection();
            SqlCommand cmds = new SqlCommand("Select Id from Rubric", cons);
            SqlDataAdapter das = new SqlDataAdapter(cmds);
            DataTable dts = new DataTable();
            das.Fill(dts);
            comboBox2.ValueMember = "Id";
            comboBox2.DataSource = dts;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string texts = textBox9.Text;
            if (comboBox6.Text != "")
            {
                if (comboBox6.Text == "Id")
                {
                    SqlCommand cmd = new SqlCommand("Select * from Clo where Id" + " LIKE '%" + texts + "%'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                if (comboBox6.Text == "Name")
                {
                    SqlCommand cmd = new SqlCommand("Select * from Clo where Name" + " LIKE '%" + texts + "%'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                if (comboBox6.Text == "DateCreated")
                {
                    SqlCommand cmd = new SqlCommand("Select * from Clo where DateCreated" + " LIKE '%" + texts + "%'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                if (comboBox6.Text == "DateUpdated")
                {
                    SqlCommand cmd = new SqlCommand("Select * from Clo where DateUpdated" + " LIKE '%" + texts + "%'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }

            }
        }

        private void textBox9_Click(object sender, EventArgs e)
        {
            if (textBox9.Text == "Enter Search text here")
            {
                textBox9.Text = "";
                textBox9.ForeColor = SystemColors.WindowText;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (textBox12.Text != "" && textBox13.Text != "" && textBox10.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Insert into Assessment values (@Title,@DateCreated,@TotalMarks,@TotalWeightage)", con);
                    if (ValidateAssessName(textBox12.Text) == true)
                    {
                        cmd.Parameters.AddWithValue("@Title", textBox12.Text);
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                        if (ValidateInput(int.Parse(textBox13.Text)) == true)
                        {
                            cmd.Parameters.AddWithValue("@TotalMarks", int.Parse(textBox13.Text));

                            if (ValidateInput(int.Parse(textBox10.Text)) == true)
                            {
                                cmd.Parameters.AddWithValue("@TotalWeightage", int.Parse(textBox10.Text));
                            }
                        }
                    }
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Successfully Saved", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearAssestexts();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Assessment", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView6.DataSource = dt;
            clearAssestexts();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                int assessamentdelid = int.Parse(textBox11.Text);
                var con = Configuration.getInstance().getConnection();


                SqlCommand cmd1 = new SqlCommand("DELETE FROM StudentResult WHERE AssessmentComponentId IN (SELECT AssessmentComponentId FROM Assessment WHERE Id = @Id)", con);
                cmd1.Parameters.AddWithValue("@Id", assessamentdelid);
                cmd1.ExecuteNonQuery();


                SqlCommand cmd2 = new SqlCommand("DELETE FROM AssessmentComponent WHERE AssessmentId = @Id", con);
                cmd2.Parameters.AddWithValue("@Id", assessamentdelid);
                cmd2.ExecuteNonQuery();

                SqlCommand cmd3 = new SqlCommand("DELETE FROM Assessment WHERE Id = @Id", con);
                cmd3.Parameters.AddWithValue("@Id", assessamentdelid);
                cmd3.ExecuteNonQuery();
                MaterialMessageBox.Show("Successfully Deleted", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearAssestexts();
            }
            catch (Exception)
            {
            }




        }

        private void button19_Click(object sender, EventArgs e)
        {
            assessamentdelid = int.Parse(textBox11.Text);
            if (textBox12.Text != "" && textBox13.Text != "" && textBox10.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Update Assessment Set Title=@Title,DateCreated=@DateCreated,TotalMarks=@TotalMarks,TotalWeightage=@TotalWeightage Where Id=@Id", con);
                    if (ValidateAssessName(textBox12.Text) == true)
                    {
                        cmd.Parameters.AddWithValue("@Title", textBox12.Text);
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                        if (ValidateInput(int.Parse(textBox13.Text)) == true)
                        {
                            cmd.Parameters.AddWithValue("@TotalMarks", int.Parse(textBox13.Text));

                            if (ValidateInput(int.Parse(textBox10.Text)) == true)
                            {
                                cmd.Parameters.AddWithValue("@TotalWeightage", int.Parse(textBox10.Text));
                            }
                        }
                    }
                    cmd.Parameters.AddWithValue("@Id", assessamentdelid);
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Successfully Updated", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearAssestexts();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                textBox11.Text = dataGridView6.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                textBox12.Text = dataGridView6.Rows[e.RowIndex].Cells["Title"].Value.ToString();
                textBox13.Text = dataGridView6.Rows[e.RowIndex].Cells["TotalMarks"].Value.ToString();
                textBox10.Text = dataGridView6.Rows[e.RowIndex].Cells["TotalWeightage"].Value.ToString();
            }
            catch (Exception)
            {

            }
        }
        public bool CheckAssessmentComponentName(int assessmentId, string componentName, SqlConnection connection)
        {
            string query = "SELECT COUNT(*) FROM AssessmentComponent WHERE AssessmentId = @assessmentId AND Name = @componentName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@assessmentId", assessmentId);
            command.Parameters.AddWithValue("@componentName", componentName);
            int count = (int)command.ExecuteScalar();
            if (count > 0)
            {
                throw new ArgumentException("An assessment component with this name already exists for this assessment.");
            }
            return true;
        }



        private void button21_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            if (textBox15.Text != "" && comboBox8.Text != "" && textBox17.Text != "" && comboBox7.Text != "")
            {
                try
                {
                    int InAS_Count = 0;
                    SqlCommand temp = new SqlCommand("SELECT SUM(AC.TotalMarks)\r\nFROM AssessmentComponent AC\r\nWHERE AC.AssessmentId = " + comboBox7.Text, con);
                    try
                    {
                        InAS_Count = Convert.ToInt32(temp.ExecuteScalar());
                    }
                    catch { }
                    SqlCommand temp1 = new SqlCommand("SELECT A.TotalMarks FROM Assessment A WHERE A.Id = " + comboBox7.Text, con);
                    int total = Convert.ToInt32(temp1.ExecuteScalar());
                    if (total - InAS_Count >= int.Parse(textBox17.Text))
                    {
                        // Check if a record with the same name, rubric id, and assessment id already exists in the database
                        SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM AssessmentComponent WHERE Name=@Name AND AssessmentId=@AssessmentId", con);
                        checkCmd.Parameters.AddWithValue("@Name", textBox15.Text);
                        checkCmd.Parameters.AddWithValue("@AssessmentId", comboBox7.Text);
                        int recordCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        // If a record already exists, show an error message
                        if (recordCount > 0)
                        {
                            MaterialMessageBox.Show("A record with the same name and assessment id already exists in the database.");
                        }
                        else
                        {
                            // Otherwise, insert the record into the database
                            SqlCommand cmd = new SqlCommand("Insert into AssessmentComponent values (@Name,@RubricId,@TotalMarks,@DateCreated,@DateUpdated,@AssessmentId)", con);
                            cmd.Parameters.AddWithValue("@Name", textBox15.Text);
                            cmd.Parameters.AddWithValue("@RubricId", comboBox8.Text);
                            if (ValidateInput(int.Parse(textBox17.Text)) == true)
                            {
                                cmd.Parameters.AddWithValue("@TotalMarks", int.Parse(textBox17.Text));
                            }
                            cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                            cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                            cmd.Parameters.AddWithValue("@AssessmentId", comboBox7.Text);
                            cmd.ExecuteNonQuery();
                            MaterialMessageBox.Show("Successfully Saved", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clearAssesscoptexts();
                        }
                    }
                    else
                    {
                        MaterialMessageBox.Show("Sum is greater than assessment marks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



        }

        private void button22_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from AssessmentComponent", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView7.DataSource = dt;
            clearAssesscoptexts();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            assesscomponentdelid = int.Parse(textBox14.Text);
            var con = Configuration.getInstance().getConnection();
            if (textBox15.Text != "" && comboBox8.Text != "" && textBox17.Text != "" && comboBox7.Text != "")
            {
                try
                {
                    int InAS_Count = 0;
                    SqlCommand temp = new SqlCommand("SELECT SUM(AC.TotalMarks)\r\nFROM AssessmentComponent AC\r\nWHERE AC.AssessmentId = " + comboBox7.Text, con);
                    try
                    {
                        InAS_Count = Convert.ToInt32(temp.ExecuteScalar());
                    }
                    catch { }
                    SqlCommand temp1 = new SqlCommand("SELECT A.TotalMarks FROM Assessment A WHERE A.Id = " + comboBox7.Text, con);
                    int total = Convert.ToInt32(temp1.ExecuteScalar());
                    if (total - InAS_Count >= int.Parse(textBox17.Text))
                    {
                        // Check if a record with the same name, rubric id, and assessment id already exists in the database
                        SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM AssessmentComponent WHERE Name=@Name AND AssessmentId=@AssessmentId AND Id<>@Id", con);
                        checkCmd.Parameters.AddWithValue("@Name", textBox15.Text);
                        checkCmd.Parameters.AddWithValue("@AssessmentId", comboBox7.Text);
                        checkCmd.Parameters.AddWithValue("@Id", assesscomponentdelid);
                        int recordCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        // If a record already exists, show an error message
                        if (recordCount > 0)
                        {
                            MaterialMessageBox.Show("A record with the same name and assessment id already exists in the database.");
                        }
                        else
                        {
                            SqlCommand cmd = new SqlCommand("Update AssessmentComponent Set Name=@Name,RubricId=@RubricId,TotalMarks=@TotalMarks,DateUpdated=@DateUpdated,AssessmentId=@AssessmentId Where Id=@Id", con);
                            cmd.Parameters.AddWithValue("@Name", textBox15.Text);
                            cmd.Parameters.AddWithValue("@RubricId", comboBox8.Text);
                            cmd.Parameters.AddWithValue("@TotalMarks", int.Parse(textBox17.Text));
                            cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                            cmd.Parameters.AddWithValue("@AssessmentId", comboBox7.Text);
                            cmd.Parameters.AddWithValue("@Id", assesscomponentdelid);
                            cmd.ExecuteNonQuery();
                            MaterialMessageBox.Show("Successfully Updated", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clearAssesscoptexts();
                        }
                    }
                    else
                    {
                        MaterialMessageBox.Show("Sum is greater than assessment marks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox8_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Rubric", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox8.ValueMember = "Id";
            comboBox8.DataSource = dt;
        }

        private void comboBox7_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Assessment", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox7.ValueMember = "Id";
            comboBox7.DataSource = dt;
        }

        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox14.Text = dataGridView7.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                textBox15.Text = dataGridView7.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                comboBox8.Text = dataGridView7.Rows[e.RowIndex].Cells["RubricId"].Value.ToString();
                textBox17.Text = dataGridView7.Rows[e.RowIndex].Cells["TotalMarks"].Value.ToString();
                comboBox7.Text = dataGridView7.Rows[e.RowIndex].Cells["AssessmentId"].Value.ToString();
            }
            catch (Exception)
            {
            }
            assesscomponentdelid = int.Parse(textBox14.Text);
        }

        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        int count = 0;
        DateTime date = DateTime.Now;
        private void button14_Click(object sender, EventArgs e)
        {
            date = Convert.ToDateTime(dateTimePicker2.Text);
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select max(Id) from ClassAttendance", con1);
            Ids = (Int32)cmd1.ExecuteScalar();
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select distinct RegistrationNumber , FirstName + ' ' + LastName As Name,(select Name from Lookup where LookupId=StudentAttendance.AttendanceStatus) As Status From Student JOIN StudentAttendance on Student.Id=StudentAttendance.StudentId JOIN ClassAttendance on ClassAttendance.Id=StudentAttendance.AttendanceId  Where Status = 5 and AttendanceDate=@AttendanceDate", con);
            cmd.Parameters.AddWithValue("@Id", Ids);
            cmd.Parameters.AddWithValue("@AttendanceDate", date);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView13.DataSource = dt;
        }
        public void attendance()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id,RegistrationNumber,FirstName+' '+LastName as Name from Student where Status='5'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataGridViewCheckBoxColumn checkBoxColumn1 = new DataGridViewCheckBoxColumn();
            checkBoxColumn1.HeaderText = "Present";
            checkBoxColumn1.Width = 50;
            checkBoxColumn1.Name = "checkBoxColumn1";
            checkBoxColumn1.TrueValue = "Yes";
            checkBoxColumn1.FalseValue = "No";

            // Create second checkbox column
            DataGridViewCheckBoxColumn checkBoxColumn2 = new DataGridViewCheckBoxColumn();
            checkBoxColumn2.HeaderText = "Absent";
            checkBoxColumn2.Width = 50;
            checkBoxColumn2.Name = "checkBoxColumn2";
            checkBoxColumn2.TrueValue = "On";
            checkBoxColumn2.FalseValue = "Off";

            // Add columns to DataGridView control
            DataGridViewCheckBoxColumn checkBoxColumn3 = new DataGridViewCheckBoxColumn();
            checkBoxColumn3.HeaderText = "Leave";
            checkBoxColumn3.Width = 50;
            checkBoxColumn3.Name = "checkBoxColumn3";
            checkBoxColumn3.TrueValue = "On";
            checkBoxColumn3.FalseValue = "Off";

            DataGridViewCheckBoxColumn checkBoxColumn4 = new DataGridViewCheckBoxColumn();
            checkBoxColumn4.HeaderText = "Late";
            checkBoxColumn4.Width = 50;
            checkBoxColumn4.Name = "checkBoxColumn4";
            checkBoxColumn4.TrueValue = "On";
            checkBoxColumn4.FalseValue = "Off";
            if (count == 0)
            {
                dataGridView5.DataSource = dt;
                dataGridView5.Columns.Add(checkBoxColumn1);
                dataGridView5.Columns.Add(checkBoxColumn2);
                dataGridView5.Columns.Add(checkBoxColumn3);
                dataGridView5.Columns.Add(checkBoxColumn4);
                count++;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("", con);


        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var date = Convert.ToDateTime(dateTimePicker2.Text);
            int Id = 0;
            var con = Configuration.getInstance().getConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("Select Id from ClassAttendance Where AttendanceDate=@Date", con);
                cmd.Parameters.AddWithValue("@Date", date);
                Id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                Id = 0;
            }

            if (Id == 0)
            {
                MaterialMessageBox.Show("Date has not added");
            }
            else
            {
                string student = dataGridView5.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                SqlCommand cmd2 = new SqlCommand("Select Id from ClassAttendance where AttendanceDate=@date", con);
                cmd2.Parameters.AddWithValue("@date", date);
                int adateid = Convert.ToInt32(cmd2.ExecuteScalar());
                SqlCommand cmd3 = new SqlCommand("Select StudentId from StudentAttendance where AttendanceId=@CA and StudentId=@Id", con);
                cmd3.Parameters.AddWithValue("@CA", adateid);
                cmd3.Parameters.AddWithValue("@Id", student);
                int scheck = Convert.ToInt32(cmd3.ExecuteScalar());
                if (e.ColumnIndex == 0)
                {
                    dataGridView5.Rows[e.RowIndex].Cells[1].Value = false;
                    dataGridView5.Rows[e.RowIndex].Cells[2].Value = false;
                    dataGridView5.Rows[e.RowIndex].Cells[3].Value = false;
                    if (scheck == 0)
                    {
                        SqlCommand cmd1 = new SqlCommand("INSERT into StudentAttendance values(@AttendanceId,@StudentId,@AttendanceStatus)", con);
                        cmd1.Parameters.AddWithValue("@AttendanceId", adateid);
                        cmd1.Parameters.AddWithValue("@StudentId", student);
                        cmd1.Parameters.AddWithValue("@AttendanceStatus", 1);
                        cmd1.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand cmd1 = new SqlCommand("UPDATE StudentAttendance SET AttendanceStatus=@AttendanceStatus where AttendanceId=@AttendanceId and StudentId=@StudentId", con);
                        cmd1.Parameters.AddWithValue("@AttendanceId", adateid);
                        cmd1.Parameters.AddWithValue("@StudentId", student);
                        cmd1.Parameters.AddWithValue("@AttendanceStatus", 1);
                        cmd1.ExecuteNonQuery();
                    }
                }
                if (e.ColumnIndex == 1)
                {
                    dataGridView5.Rows[e.RowIndex].Cells[0].Value = false;
                    dataGridView5.Rows[e.RowIndex].Cells[2].Value = false;
                    dataGridView5.Rows[e.RowIndex].Cells[3].Value = false;
                    if (scheck == 0)
                    {
                        SqlCommand cmd1 = new SqlCommand("INSERT into StudentAttendance values(@AttendanceId,@StudentId,@AttendanceStatus)", con);
                        cmd1.Parameters.AddWithValue("@AttendanceId", adateid);
                        cmd1.Parameters.AddWithValue("@StudentId", student);
                        cmd1.Parameters.AddWithValue("@AttendanceStatus", 2);
                        cmd1.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand cmd1 = new SqlCommand("UPDATE StudentAttendance SET AttendanceStatus=@AttendanceStatus where AttendanceId=@AttendanceId and StudentId=@StudentId", con);
                        cmd1.Parameters.AddWithValue("@AttendanceId", adateid);
                        cmd1.Parameters.AddWithValue("@StudentId", student);
                        cmd1.Parameters.AddWithValue("@AttendanceStatus", 2);
                        cmd1.ExecuteNonQuery();
                    }
                }
                if (e.ColumnIndex == 2)
                {
                    dataGridView5.Rows[e.RowIndex].Cells[1].Value = false;
                    dataGridView5.Rows[e.RowIndex].Cells[0].Value = false;
                    dataGridView5.Rows[e.RowIndex].Cells[3].Value = false;
                    if (scheck == 0)
                    {
                        SqlCommand cmd1 = new SqlCommand("INSERT into StudentAttendance values(@AttendanceId,@StudentId,@AttendanceStatus)", con);
                        cmd1.Parameters.AddWithValue("@AttendanceId", adateid);
                        cmd1.Parameters.AddWithValue("@StudentId", student);
                        cmd1.Parameters.AddWithValue("@AttendanceStatus", 3);
                        cmd1.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand cmd1 = new SqlCommand("UPDATE StudentAttendance SET AttendanceStatus=@AttendanceStatus where AttendanceId=@AttendanceId and StudentId =@StudentId", con);
                        cmd1.Parameters.AddWithValue("@AttendanceId", adateid);
                        cmd1.Parameters.AddWithValue("@StudentId", student);
                        cmd1.Parameters.AddWithValue("@AttendanceStatus", 3);
                        cmd1.ExecuteNonQuery();
                    }
                }
                if (e.ColumnIndex == 3)
                {
                    dataGridView5.Rows[e.RowIndex].Cells[1].Value = false;
                    dataGridView5.Rows[e.RowIndex].Cells[2].Value = false;
                    dataGridView5.Rows[e.RowIndex].Cells[0].Value = false;
                    if (scheck == 0)
                    {
                        SqlCommand cmd1 = new SqlCommand("INSERT into StudentAttendance values(@AttendanceId,@StudentId,@AttendanceStatus)", con);
                        cmd1.Parameters.AddWithValue("@AttendanceId", adateid);
                        cmd1.Parameters.AddWithValue("@StudentId", student);
                        cmd1.Parameters.AddWithValue("@AttendanceStatus", 4);
                        cmd1.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand cmd1 = new SqlCommand("UPDATE StudentAttendance SET AttendanceStatus=@AttendanceStatus where AttendanceId=@AttendanceId and StudentId=@StudentId", con);
                        cmd1.Parameters.AddWithValue("@AttendanceId", adateid);
                        cmd1.Parameters.AddWithValue("@StudentId", student);
                        cmd1.Parameters.AddWithValue("@AttendanceStatus", 4);
                        cmd1.ExecuteNonQuery();
                    }
                }
            }
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dates = Convert.ToDateTime(dateTimePicker2.Text);
            if (dates <= DateTime.Now)
            {
                SqlCommand cmd1 = new SqlCommand("Select Id from ClassAttendance where AttendanceDate=@Date", con);
                cmd1.Parameters.AddWithValue("@Date", dates);
                int id = Convert.ToInt32(cmd1.ExecuteScalar());
                if (id == 0)
                {
                    SqlCommand cmd = new SqlCommand("INSERT Into ClassAttendance Values(@AttendanceDate)", con);
                    cmd.Parameters.AddWithValue("@AttendanceDate", dates);
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Date has been Added", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MaterialMessageBox.Show("Date has Already Added", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MaterialMessageBox.Show("Date Can't be greater than " + date, "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dataGridView5.Refresh();
        }

        private void materialTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RubricLevel();
            Assessments();
            Rubrics();
            CLO();
            loadActive();
            loadInActive();
            rubricidgetter();
        }

        private void tableLayoutPanel17_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txtaddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtFname_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtlname_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage9_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel18_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            string texts = textBox8.Text;
            try
            {
                if (textBox8.Text != "")
                {

                    var con1 = Configuration.getInstance().getConnection();
                    SqlCommand cmd1 = new SqlCommand("Select max(Id) from ClassAttendance", con1);
                    Ids = (Int32)cmd1.ExecuteScalar();
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Select distinct RegistrationNumber , FirstName + ' ' + LastName As Name,(select Name from Lookup where LookupId=StudentAttendance.AttendanceStatus) As Status From Student JOIN StudentAttendance on Student.Id=StudentAttendance.StudentId JOIN ClassAttendance on ClassAttendance.Id=StudentAttendance.AttendanceId  Where Status = 5 and AttendanceDate=@AttendanceDate and (FirstName + ' ' + LastName)" + " LIKE '%" + texts + "%'", con);
                    cmd.Parameters.AddWithValue("@Id", Ids);
                    //MaterialMessageBox.Show(date.ToString());
                    cmd.Parameters.AddWithValue("@AttendanceDate", date);
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView13.DataSource = dt;
                }
                else
                {
                    var con1 = Configuration.getInstance().getConnection();
                    SqlCommand cmd1 = new SqlCommand("Select max(Id) from ClassAttendance", con1);
                    Ids = (Int32)cmd1.ExecuteScalar();
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Select distinct RegistrationNumber , FirstName + ' ' + LastName As Name,(select Name from Lookup where LookupId=StudentAttendance.AttendanceStatus) As Status From Student JOIN StudentAttendance on Student.Id=StudentAttendance.StudentId JOIN ClassAttendance on ClassAttendance.Id=StudentAttendance.AttendanceId  Where Status = 5 and AttendanceDate=@AttendanceDate", con);
                    cmd.Parameters.AddWithValue("@Id", Ids);
                    cmd.Parameters.AddWithValue("@AttendanceDate", date);
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView13.DataSource = dt;
                }
            }
            catch (Exception)
            {

            }
        }

        private void tableLayoutPanel19_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel22_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel20_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel26_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {

            try
            {
                int delassesmentcompoid = int.Parse(textBox14.Text);
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("Delete from StudentResult where AssessmentComponentId=@Id", con);
                cmd1.Parameters.AddWithValue("@Id", delassesmentcompoid);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd = new SqlCommand("Delete from AssessmentComponent Where Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", delassesmentcompoid);
                cmd.ExecuteNonQuery();
                MaterialMessageBox.Show("Successfully Deleted", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearAssesscoptexts();
            }
            catch (Exception)
            {
            }


        }

        private void tableLayoutPanel25_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel24_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel23_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void tabPage8_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel27_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView8_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView9_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel28_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void materialTabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            RubricLevel();
            Assessments();
            Rubrics();
            CLO();
            loadActive();
            loadInActive();
            rubricidgetter();
        }

        private void dataGridView13_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox8_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox8.Text == "Enter name here to search")
            {
                textBox8.Text = "";
                textBox8.ForeColor = SystemColors.WindowText;
            }
        }

        private void dataGridView8_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox16.Text = dataGridView8.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void comboBox9_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("  Select distinct RL.Id from RubricLevel RL join Rubric R on  R.Id=RL.RubricId join AssessmentComponent AC on R.Id=AC.RubricId Where AC.Id=" + int.Parse(comboBox9.Text), con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox10.ValueMember = "Id";
            comboBox10.DataSource = dt;
        }

        private void comboBox9_MouseClick(object sender, MouseEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Assessmentcomponent", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox9.ValueMember = "Id";
            comboBox9.DataSource = dt;
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (textBox16.Text != "" && comboBox9.Text != "" && comboBox10.Text != "")
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Insert into StudentResult values (@StudentId,@AssessmentComponentId,@RubricMeasurementId,@EvaluationDate)", con);
                    cmd.Parameters.AddWithValue("@StudentId", int.Parse(textBox16.Text));
                    cmd.Parameters.AddWithValue("@AssessmentComponentId", int.Parse(comboBox9.Text));
                    cmd.Parameters.AddWithValue("@RubricMeasurementId", int.Parse(comboBox10.Text));
                    cmd.Parameters.AddWithValue("@EvaluationDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Successfully Marked", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearEval();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MaterialMessageBox.Show("Enter All Credentials", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void comboBox10_Click(object sender, EventArgs e)
        {
            //var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("Select Id from RubricLevel join Rubric on RubricLevel.RubricId=Rubric.Id join AssessmentComponent on Rubric.Id=AssessmentComponent.RubticId where Assessment.Id="ComboBox9.Text,con);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            //comboBox10.ValueMember = "Id";
            //comboBox10.DataSource = dt;
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox16.Text != "" && comboBox9.Text != "" && comboBox10.Text != "")
                {
                    int sid = int.Parse(textBox16.Text);
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Update StudentResult set AssessmentComponentId=@AssessmentComponentId,RubricMeasurementId=@RubricMeasurementId,EvaluationDate=@EvaluationDate where StudentId=@StudentId", con);
                    cmd.Parameters.AddWithValue("@AssessmentComponentId", int.Parse(comboBox9.Text));
                    cmd.Parameters.AddWithValue("@RubricMeasurementId", int.Parse(comboBox10.Text));
                    cmd.Parameters.AddWithValue("@EvaluationDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@StudentId", sid);
                    cmd.ExecuteNonQuery();
                    MaterialMessageBox.Show("Successfully Updated", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearEval();
                }
                else
                {
                    MaterialMessageBox.Show("Enter All Credentials", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
            }



        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Name, RegistrationNumber, ObtainedMarks, TotalMarks, AssessmentComponentId, RubricMeasurementId, AssessmentComponent, Title FROM(SELECT FirstName + ' ' + LastName AS Name, RegistrationNumber, (CONVERT(FLOAT, RubricLevel.MeasurementLevel) / MAX(RubricLevel.MeasurementLevel) OVER() * AssessmentComponent.TotalMarks) AS ObtainedMarks, AssessmentComponent.TotalMarks, AssessmentComponentId, RubricMeasurementId,AssessmentComponent.Name AS AssessmentComponent, Assessment.Title FROM Student JOIN StudentResult ON Student.Id = StudentResult.StudentId JOIN RubricLevel ON StudentResult.RubricMeasurementId = RubricLevel.Id JOIN Rubric ON RubricLevel.RubricId = Rubric.Id JOIN AssessmentComponent ON Rubric.Id = AssessmentComponent.RubricId JOIN Assessment ON AssessmentComponent.AssessmentId = Assessment.Id WHERE StudentResult.StudentId = StudentId AND AssessmentComponent.Id = AssessmentComponentId) AS subquery", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView9.DataSource = dt;
            clearEval();
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            string textx = materialComboBox1.Text;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT SubQuery.RegistrationNumber, SubQuery.Name, SubQuery.TotalMarks, SUM(SubQuery.ObtainedMarks) AS ObtainedMarks, SubQuery.TotalWeightage, SUM(SubQuery.ObtainedWeightage) AS ObtainedWeightage FROM(SELECT DISTINCT S.RegistrationNumber, S.FirstName + '' + S.LastName AS Name, A.Title, A.TotalMarks, A.Totalweightage, (CONVERT(FLOAT, RL.MeasurementLevel) / MAX(RL.MeasurementLevel) OVER() * AC.TotalMarks) AS ObtainedMarks, ((CONVERT(FLOAT, RL.MeasurementLevel) / MAX(RL.MeasurementLevel) OVER() * AC.TotalMarks) / A.TotalMarks * A.TotalWeightage) AS ObtainedWeightage FROM Student S JOIN StudentResult SR ON S.Id = SR.StudentId JOIN RubricLevel RL ON SR.RubricMeasurementId = RL.Id JOIN Rubric R ON RL.RubricId = R.Id JOIN AssessmentComponent AC ON R.Id = AC.RubricId JOIN Assessment A ON AC.AssessmentId = A.Id WHERE SR.StudentId = [StudentId] AND AC.Id = [AssessmentComponentId] AND A.Title ='" + textx + "') AS SubQuery GROUP BY SubQuery.RegistrationNumber, SubQuery.Name, SubQuery.TotalMarks, SubQuery.TotalWeightage", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView10.DataSource = dt;



        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            string textx = materialComboBox2.Text;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT RegistrationNumber, Name, SUM(TotalMarks) AS TotalMarks, SUM(ObtainedMarks) AS ObtainedMarks FROM( SELECT S.RegistrationNumber, S.FirstName + ' ' + S.LastName AS Name, Clo.Name AS[CLO Name], Clo.Id, A.Title, AC.Name AS[Assessment Component Name], AC.TotalMarks AS TotalMarks, A.Totalweightage, (CONVERT(FLOAT, RL.MeasurementLevel) / MAX(RL.MeasurementLevel) OVER() * AC.TotalMarks) AS ObtainedMarks, ((CONVERT(FLOAT, RL.MeasurementLevel) / MAX(RL.MeasurementLevel) OVER() * AC.TotalMarks) / A.TotalMarks * A.TotalWeightage) AS ObtainedWeightage FROM Student S INNER JOIN StudentResult SR ON S.Id = SR.StudentId INNER JOIN RubricLevel RL ON SR.RubricMeasurementId = RL.Id INNER JOIN Rubric R ON RL.RubricId = R.Id INNER JOIN Clo ON R.CloId = Clo.Id INNER JOIN AssessmentComponent AC ON R.Id = AC.RubricId INNER JOIN Assessment A ON AC.AssessmentId = A.Id WHERE SR.StudentId = [StudentId] AND AC.Id = [AssessmentComponentId]) AS NewTable WHERE [CLO Name] ='" + textx + "'GROUP BY RegistrationNumber, Name, Title", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView11.DataSource = dt;
        }

        private void materialComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void materialComboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Name from Clo", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            materialComboBox2.ValueMember = "Name";
            materialComboBox2.DataSource = dt;
        }

        private void materialComboBox1_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Title from Assessment", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            materialComboBox1.ValueMember = "Title";
            materialComboBox1.DataSource = dt;
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {

            if (materialComboBox1.SelectedIndex == -1)
            {
                MaterialMessageBox.Show("Select Value from Combobox first");
            }
            else
            {
                GenerateReport();
            }
        }
        public void GenerateReport()
        {
            // Define variables
            DataTable dataTable = new DataTable();
            string[] headers = { "Registration Number", "Name", "Total Marks", "Obtained Marks", "Total Weightage", "Obtained Weightage" };

            // Execute query and fill DataTable
            string textx = materialComboBox1.Text;
            // Execute query and fill DataTable
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT SubQuery.RegistrationNumber, SubQuery.Name, SubQuery.TotalMarks, SUM(SubQuery.ObtainedMarks) AS ObtainedMarks, SubQuery.TotalWeightage, SUM(SubQuery.ObtainedWeightage) AS ObtainedWeightage FROM(SELECT DISTINCT S.RegistrationNumber, S.FirstName + '' + S.LastName AS Name, A.Title, A.TotalMarks, A.Totalweightage, (CONVERT(FLOAT, RL.MeasurementLevel) / MAX(RL.MeasurementLevel) OVER() * AC.TotalMarks) AS ObtainedMarks, ((CONVERT(FLOAT, RL.MeasurementLevel) / MAX(RL.MeasurementLevel) OVER() * AC.TotalMarks) / A.TotalMarks * A.TotalWeightage) AS ObtainedWeightage FROM Student S JOIN StudentResult SR ON S.Id = SR.StudentId JOIN RubricLevel RL ON SR.RubricMeasurementId = RL.Id JOIN Rubric R ON RL.RubricId = R.Id JOIN AssessmentComponent AC ON R.Id = AC.RubricId JOIN Assessment A ON AC.AssessmentId = A.Id WHERE SR.StudentId = [StudentId] AND AC.Id = [AssessmentComponentId] AND A.Title ='" + textx + "') AS SubQuery GROUP BY SubQuery.RegistrationNumber, SubQuery.Name, SubQuery.TotalMarks, SubQuery.TotalWeightage", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView10.DataSource = dt;


            // Create PDF document and set margins
            Document doc = new Document();

            // Create a new PDF writer
            PdfWriter.GetInstance(doc, new FileStream(textx + " " + "AssessmentWise_report.pdf", FileMode.Create));

            // Open the document
            doc.Open();
            PdfPTable headingTable = new PdfPTable(1);
            headingTable.WidthPercentage = 100;
            PdfPCell headingCell = new PdfPCell(new Phrase("Assessment Report - " + textx));
            headingCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headingCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headingCell.Padding = 8;
            headingCell.BackgroundColor = new BaseColor(204, 204, 204);
            headingTable.AddCell(headingCell);
            doc.Add(headingTable);

            // Create a new table with the same number of columns as the DataGridView
            PdfPTable table = new PdfPTable(dataGridView10.ColumnCount);
            table.WidthPercentage = 100;

            // Add the column headers to the table
            for (int i = 0; i < dataGridView10.ColumnCount; i++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(dataGridView10.Columns[i].HeaderText));
                cell.BackgroundColor = new BaseColor(204, 204, 204); // gray background color
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Padding = 8;
                table.AddCell(cell);
            }

            // Add the rows to the table
            for (int i = 0; i < dataGridView10.RowCount; i++)
            {
                for (int j = 0; j < dataGridView10.ColumnCount; j++)
                {
                    if (dataGridView10[j, i].Value != null)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataGridView10[j, i].Value.ToString()));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 8;
                        table.AddCell(cell);
                    }
                }
            }

            // Add table to document
            doc.Add(table);

            // Close document and clean up resources
            doc.Close();
            MaterialMessageBox.Show("Downloaded Successfully", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void materialButton7_Click(object sender, EventArgs e)
        {
            if (materialComboBox2.SelectedIndex == -1)
            {
                MaterialMessageBox.Show("Select Value from Combobox first");
            }
            else
            {
                gen();
            }
        }
        public void gen()
        {
            var dates = date.ToString("yyyy-MM-dd");
            DataTable dataTable = new DataTable();
            string[] headers = { "Registration Number", "Name", "Total Marks", "Obtained Marks", };
            // Define variables
            string textx = materialComboBox2.Text;
            // Execute query and fill DataTable
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT RegistrationNumber, Name, SUM(TotalMarks) AS TotalMarks, SUM(ObtainedMarks) AS ObtainedMarks FROM( SELECT S.RegistrationNumber, S.FirstName + ' ' + S.LastName AS Name, Clo.Name AS[CLO Name], Clo.Id, A.Title, AC.Name AS[Assessment Component Name], AC.TotalMarks AS TotalMarks, A.Totalweightage, (CONVERT(FLOAT, RL.MeasurementLevel) / MAX(RL.MeasurementLevel) OVER() * AC.TotalMarks) AS ObtainedMarks, ((CONVERT(FLOAT, RL.MeasurementLevel) / MAX(RL.MeasurementLevel) OVER() * AC.TotalMarks) / A.TotalMarks * A.TotalWeightage) AS ObtainedWeightage FROM Student S INNER JOIN StudentResult SR ON S.Id = SR.StudentId INNER JOIN RubricLevel RL ON SR.RubricMeasurementId = RL.Id INNER JOIN Rubric R ON RL.RubricId = R.Id INNER JOIN Clo ON R.CloId = Clo.Id INNER JOIN AssessmentComponent AC ON R.Id = AC.RubricId INNER JOIN Assessment A ON AC.AssessmentId = A.Id WHERE SR.StudentId = [StudentId] AND AC.Id = [AssessmentComponentId]) AS NewTable WHERE [CLO Name] ='" + textx + "'GROUP BY RegistrationNumber, Name, Title", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView11.DataSource = dt;

            // Create a new PDF document
            Document doc = new Document();

            // Create a new PDF writer
            PdfWriter.GetInstance(doc, new FileStream(textx + " " + "CLO_report.pdf", FileMode.Create));

            // Open the document
            doc.Open();
            // Add heading with current date and time
            PdfPTable headingTable = new PdfPTable(1);
            headingTable.WidthPercentage = 100;

            PdfPCell headingCell = new PdfPCell(new Phrase("CLO_Report - " + textx));
            headingCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headingCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headingCell.Padding = 8;
            headingCell.BackgroundColor = new BaseColor(204, 204, 204);
            headingTable.AddCell(headingCell);

            doc.Add(headingTable);

            // Create a new table with the same number of columns as the DataGridView
            PdfPTable table = new PdfPTable(dataGridView11.ColumnCount);
            table.WidthPercentage = 100;

            // Add the column headers to the table
            for (int i = 0; i < dataGridView11.ColumnCount; i++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(dataGridView11.Columns[i].HeaderText));
                cell.BackgroundColor = new BaseColor(204, 204, 204); // gray background color
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Padding = 8;
                table.AddCell(cell);
            }

            // Add the rows to the table
            for (int i = 0; i < dataGridView11.RowCount; i++)
            {
                for (int j = 0; j < dataGridView11.ColumnCount; j++)
                {
                    if (dataGridView11[j, i].Value != null)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataGridView11[j, i].Value.ToString()));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 8;
                        table.AddCell(cell);
                    }
                }
            }

            // Add table to document
            doc.Add(table);

            // Close document and clean up resources
            doc.Close();
            MaterialMessageBox.Show("Downloaded Successfully", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void gen2()
        {
            DataTable dataTable = new DataTable();
            string[] headers = { "Registration Number", "Name", "Total Marks", "Obtained Marks", };
            // Define variables
            string textx = materialComboBox2.Text;
            // Execute query and fill DataTable
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT RegistrationNumber, Name, SUM(TotalMarks) AS TotalMarks, SUM(ObtainedMarks) AS ObtainedMarks FROM( SELECT S.RegistrationNumber, S.FirstName + ' ' + S.LastName AS Name, Clo.Name AS[CLO Name], Clo.Id, A.Title, AC.Name AS[Assessment Component Name], AC.TotalMarks AS TotalMarks, A.Totalweightage, (CONVERT(FLOAT, RL.MeasurementLevel) / MAX(RL.MeasurementLevel) OVER() * AC.TotalMarks) AS ObtainedMarks, ((CONVERT(FLOAT, RL.MeasurementLevel) / MAX(RL.MeasurementLevel) OVER() * AC.TotalMarks) / A.TotalMarks * A.TotalWeightage) AS ObtainedWeightage FROM Student S INNER JOIN StudentResult SR ON S.Id = SR.StudentId INNER JOIN RubricLevel RL ON SR.RubricMeasurementId = RL.Id INNER JOIN Rubric R ON RL.RubricId = R.Id INNER JOIN Clo ON R.CloId = Clo.Id INNER JOIN AssessmentComponent AC ON R.Id = AC.RubricId INNER JOIN Assessment A ON AC.AssessmentId = A.Id WHERE SR.StudentId = [StudentId] AND AC.Id = [AssessmentComponentId]) AS NewTable WHERE [CLO Name] ='" + textx + "'GROUP BY RegistrationNumber, Name, Title", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView11.DataSource = dt;

            // Create a new PDF document
            Document doc = new Document();

            // Create a new PDF writer
            PdfWriter.GetInstance(doc, new FileStream("CLO_report.pdf", FileMode.Create));

            // Open the document
            doc.Open();

            // Create a new table with the same number of columns as the DataGridView
            PdfPTable table = new PdfPTable(dataGridView11.ColumnCount);

            // Add the column headers to the table
            for (int i = 0; i < dataGridView11.ColumnCount; i++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(dataGridView11.Columns[i].HeaderText));
                cell.BackgroundColor = new BaseColor(204, 204, 204); // gray background color
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Padding = 8;
                table.AddCell(cell);
            }

            // Add the rows to the table
            for (int i = 0; i < dataGridView11.RowCount; i++)
            {
                for (int j = 0; j < dataGridView11.ColumnCount; j++)
                {
                    if (dataGridView11[j, i].Value != null)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataGridView11[j, i].Value.ToString()));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 8;
                        table.AddCell(cell);
                    }
                }
            }

            // Add table to document
            doc.Add(table);

            // Close document and clean up resources
            doc.Close();
            MaterialMessageBox.Show("Downloaded Successfully", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void attenreport()
        {
            string[] headers = { "Id", "Registration Number", "Name", "Status" };
            date = Convert.ToDateTime(dateTimePicker5.Text);
            var dates = date.ToString("yyyy-MM-dd");
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select max(Id) from ClassAttendance", con1);
            Ids = (Int32)cmd1.ExecuteScalar();
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select distinct Student.Id, RegistrationNumber , FirstName + ' ' + LastName As Name,(select Name from Lookup where LookupId=StudentAttendance.AttendanceStatus) As Status From Student JOIN StudentAttendance on Student.Id=StudentAttendance.StudentId JOIN ClassAttendance on ClassAttendance.Id=StudentAttendance.AttendanceId  Where Status = 5 and AttendanceDate=@AttendanceDate", con);
            cmd.Parameters.AddWithValue("@Id", Ids);
            cmd.Parameters.AddWithValue("@AttendanceDate", date);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView12.DataSource = dt;
            Document doc = new Document();


            // Create a new PDF writer
            string temp = dateTimePicker5.Text + " " + "Attendance_Report.pdf";
            //MaterialMessageBox.Show(temp);
            PdfWriter.GetInstance(doc, new FileStream(temp, FileMode.Create));

            // Open the document
            doc.Open();
            // Add heading with current date and time
            PdfPTable headingTable = new PdfPTable(1);
            headingTable.WidthPercentage = 100;

            PdfPCell headingCell = new PdfPCell(new Phrase("Attendance Report - " + dates));
            headingCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headingCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headingCell.Padding = 8;
            headingCell.BackgroundColor = new BaseColor(204, 204, 204);
            headingTable.AddCell(headingCell);

            doc.Add(headingTable);


            // Create a new table with the same number of columns as the DataGridView
            PdfPTable table = new PdfPTable(dataGridView12.ColumnCount);
            table.WidthPercentage = 100;

            // Add the column headers to the table
            for (int i = 0; i < dataGridView12.ColumnCount; i++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(dataGridView12.Columns[i].HeaderText));
                cell.BackgroundColor = new BaseColor(204, 204, 204); // gray background color
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Padding = 8;
                table.AddCell(cell);
            }

            // Add the rows to the table
            for (int i = 0; i < dataGridView12.RowCount; i++)
            {
                for (int j = 0; j < dataGridView12.ColumnCount; j++)
                {
                    if (dataGridView12[j, i].Value != null)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataGridView12[j, i].Value.ToString()));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 8;
                        table.AddCell(cell);
                    }
                }
            }


            // Add table to document
            doc.Add(table);

            // Close document and clean up resources
            doc.Close();
            MaterialMessageBox.Show("Downloaded Successfully", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button30_Click(object sender, EventArgs e)
        {

        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            attenreport();
        }


        private void dataGridView14_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox21.Text = dataGridView14.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void materialButton9_Click(object sender, EventArgs e)
        {
            try
            {
                int acid = int.Parse(textBox21.Text);
                var con1 = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("Select LookupId from Lookup where Name='Active'", con1);
                status = (Int32)cmd1.ExecuteScalar();
                cmd1.ExecuteNonQuery();
                if (textBox21.Text != "")
                {

                    try
                    {
                        var con = Configuration.getInstance().getConnection();
                        //string Id=Convert.ToString(ProcessCmdKey)
                        SqlCommand cmd = new SqlCommand("update Student set Status=@Status where Id=@Id", con);
                        //MaterialMessageBox.Show(sid.ToString());
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@Id", acid);
                        cmd.ExecuteNonQuery();
                        MaterialMessageBox.Show("Successfully Changed Status to Active", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    MaterialMessageBox.Show("Enter a Unique Credential to Active", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {

            }
            loadinActive();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void materialComboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void materialComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void textBox6_MouseClick(object sender, MouseEventArgs e)
        {
            rubricidgetter();
        }

        private void tableLayoutPanel31_Paint(object sender, PaintEventArgs e)
        {

        }

        private void materialButton10_Click(object sender, EventArgs e)
        {
            generattentenrep();
        }
        public void generattentenrep()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("DECLARE @cols AS NVARCHAR(MAX), @query AS NVARCHAR(MAX), @MinDate DATE, @MaxDate DATE, @SQL NVARCHAR(MAX); SELECT @MinDate = MIN(AttendanceDate), @MaxDate = MAX(AttendanceDate) FROM ClassAttendance; SET @cols = ''; WHILE @MinDate <= @MaxDate BEGIN SET @cols = CONCAT(@cols, ', MAX(CASE WHEN AttendanceDate = ''', CONVERT(VARCHAR(10), @MinDate, 120), ''' THEN CASE sa.AttendanceStatus WHEN 1 THEN ''Present'' WHEN 2 THEN ''Absent'' WHEN 3 THEN ''Late'' WHEN 4 THEN ''Leave'' ELSE ''Not Marked Yet'' END ELSE NULL END) AS [', CONVERT(VARCHAR(10), @MinDate, 120), ']'); SET @MinDate = DATEADD(DAY, 1, @MinDate); END; SET @SQL = 'SELECT s.FirstName + '' '' + s.LastName AS Name' + @cols + ' FROM Student s LEFT JOIN StudentAttendance sa ON s.Id = sa.StudentId LEFT JOIN ClassAttendance ca ON ca.Id = sa.AttendanceId GROUP BY s.Id, s.FirstName, s.LastName ORDER BY s.LastName, s.FirstName'; EXEC sp_executesql @SQL;", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView12.DataSource = dt;

            // Create a new PDF document


            // Create a new PDF document
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream("Complete_Attendance_report.pdf", FileMode.Create));

            // Open the document
            doc.Open();

            // Add a title
            Paragraph title = new Paragraph("Full Attendance Report");
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;
            doc.Add(title);

            // Add a table
            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.WidthPercentage = 100f;
            table.SpacingAfter = 20f;
            table.DefaultCell.Padding = 5;
            table.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);
            table.DefaultCell.BorderColor = new BaseColor(128, 128, 128);

            // Add header cells
            foreach (DataColumn column in dt.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
            }

            // Add data cells
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(row[column].ToString()));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);
                }
            }

            doc.Add(table);

            // Close the document
            doc.Close();

            MaterialMessageBox.Show("Downloaded Successfully", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);





        }

        private void materialButton11_Click(object sender, EventArgs e)
        {
            string[] headers = { "Name", "Registration Number", "Obtained Marks", "Total Marks","AssessmentComponent Id","Rubric Measurement Id","Assessment Component","Title" };
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Name, RegistrationNumber, ObtainedMarks, TotalMarks, AssessmentComponentId, RubricMeasurementId, AssessmentComponent, Title FROM(SELECT FirstName + ' ' + LastName AS Name, RegistrationNumber, (CONVERT(FLOAT, RubricLevel.MeasurementLevel) / MAX(RubricLevel.MeasurementLevel) OVER() * AssessmentComponent.TotalMarks) AS ObtainedMarks, AssessmentComponent.TotalMarks, AssessmentComponentId, RubricMeasurementId,AssessmentComponent.Name AS AssessmentComponent, Assessment.Title FROM Student JOIN StudentResult ON Student.Id = StudentResult.StudentId JOIN RubricLevel ON StudentResult.RubricMeasurementId = RubricLevel.Id JOIN Rubric ON RubricLevel.RubricId = Rubric.Id JOIN AssessmentComponent ON Rubric.Id = AssessmentComponent.RubricId JOIN Assessment ON AssessmentComponent.AssessmentId = Assessment.Id WHERE StudentResult.StudentId = StudentId AND AssessmentComponent.Id = AssessmentComponentId) AS subquery", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView9.DataSource = dt;
            Document doc = new Document();


            // Create a new PDF writer
            string temp = "Overall Assessment Report"+".pdf";
            //MaterialMessageBox.Show(temp);
            PdfWriter.GetInstance(doc, new FileStream(temp, FileMode.Create));

            // Open the document
            doc.Open();
            // Add heading with current date and time
            PdfPTable headingTable = new PdfPTable(1);
            headingTable.WidthPercentage = 100;

            PdfPCell headingCell = new PdfPCell(new Phrase("Overall Assessment Report" ));
            headingCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headingCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headingCell.Padding = 8;
            headingCell.BackgroundColor = new BaseColor(204, 204, 204);
            headingTable.AddCell(headingCell);

            doc.Add(headingTable);


            // Create a new table with the same number of columns as the DataGridView
            PdfPTable table = new PdfPTable(dataGridView9.ColumnCount);
            table.WidthPercentage = 100;

            // Add the column headers to the table
            for (int i = 0; i < dataGridView9.ColumnCount; i++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(dataGridView9.Columns[i].HeaderText));
                cell.BackgroundColor = new BaseColor(204, 204, 204); // gray background color
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Padding = 8;
                table.AddCell(cell);
            }

            // Add the rows to the table
            for (int i = 0; i < dataGridView9.RowCount; i++)
            {
                for (int j = 0; j < dataGridView9.ColumnCount; j++)
                {
                    if (dataGridView9[j, i].Value != null)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataGridView9[j, i].Value.ToString()));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 8;
                        table.AddCell(cell);
                    }
                }
            }


            // Add table to document
            doc.Add(table);

            // Close document and clean up resources
            doc.Close();
            MaterialMessageBox.Show("Downloaded Successfully", "inform", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void comboBox10_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }

    }


    

