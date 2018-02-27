Imports System.Data.SqlClient

Imports System.IO

Public Class frmProductReport

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmProductReport_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Fill()
    End Sub
    Sub Fill()
        Try
            Dim _with1 = lvProductName
            _with1.Clear()
            _with1.Columns.Add("Product Name", 250, HorizontalAlignment.Left)
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("select distinct (Productname) from Product order by 1", con)
            rdr = cmd.ExecuteReader()
            While rdr.Read()
                Dim item = New ListViewItem()
                item.Text = rdr(0).ToString().Trim()
                lvProductName.Items.Add(item)
                For i = 0 To lvProductName.Items.Count - 1
                    lvProductName.Items(i).Checked = True
                Next
            End While
            con.Close()
            Dim _with2 = lvDepartment
            _with2.Clear()
            _with2.Columns.Add("Department", 200, HorizontalAlignment.Left)
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select distinct (DepartmentName) from Category,SubCategory,Product,Department where Category.CategoryName=SubCategory.Category and Product.SubCategoryID=SubCategory.ID and Department.DepartmentName=Product.Department order by 1", con)
            rdr = cmd.ExecuteReader()
            While rdr.Read()
                Dim item = New ListViewItem()
                item.Text = rdr(0).ToString().Trim()
                lvDepartment.Items.Add(item)
                For i = 0 To lvDepartment.Items.Count - 1
                    lvDepartment.Items(i).Checked = True
                Next
            End While
            con.Close()
            Dim _with3 = lvCategory
            _with3.Clear()
            _with3.Columns.Add("Category", 200, HorizontalAlignment.Left)
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select distinct (CategoryName) from Category,SubCategory,Product,Department where Category.CategoryName=SubCategory.Category and Product.SubCategoryID=SubCategory.ID and Department.DepartmentName=Product.Department order by 1", con)
            rdr = cmd.ExecuteReader()
            While rdr.Read()
                Dim item = New ListViewItem()
                item.Text = rdr(0).ToString().Trim()
                lvCategory.Items.Add(item)
                For i = 0 To lvCategory.Items.Count - 1
                    lvCategory.Items(i).Checked = True
                Next
            End While
            con.Close()
            Dim _with4 = lvSubCategory
            _with4.Clear()
            _with4.Columns.Add("Sub Category", 200, HorizontalAlignment.Left)
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select distinct (SubCategoryName) from Category,SubCategory,Product,Department where Category.CategoryName=SubCategory.Category and Product.SubCategoryID=SubCategory.ID and Department.DepartmentName=Product.Department order by 1", con)
            rdr = cmd.ExecuteReader()
            While rdr.Read()
                Dim item = New ListViewItem()
                item.Text = rdr(0).ToString().Trim()
                lvSubCategory.Items.Add(item)
                For i = 0 To lvSubCategory.Items.Count - 1
                    lvSubCategory.Items(i).Checked = True
                Next
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            Cursor = Cursors.WaitCursor
            Timer1.Enabled = True
            Dim Condition As String = ""
            For I = 0 To lvDepartment.CheckedItems.Count - 1

                Condition += String.Format("'{0}',", lvDepartment.CheckedItems(I).Text)
            Next
            Condition = Condition.Substring(0, Condition.Length - 1)
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select (PID),(ProductCode), (Productname),(Department), (SubCategoryID),(CategoryName),(SubCategoryName), (Description),(UPCCode), (Price),(Discount),EntryDate from Category,SubCategory,Product,Department where Category.CategoryName=SubCategory.Category and Product.SubCategoryID=SubCategory.ID and Department.DepartmentName=Product.Department and DepartmentName in (" & Condition & ")", con)
            adp = New SqlDataAdapter(cmd)
            dtable = New DataTable()
            adp.Fill(dtable)
            con.Close()
            dgw.DataSource = dtable
            ds = New DataSet()
            ds.Tables.Add(dtable)
            ds.WriteXmlSchema("ProductsReport.xml")
            Dim rpt As New rptProductReport
            rpt.SetDataSource(ds)
            frmReport.CrystalReportViewer1.ReportSource = rpt
            frmReport.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Try
            Cursor = Cursors.WaitCursor
            Timer1.Enabled = True
            Dim Condition As String = ""
            For I = 0 To lvCategory.CheckedItems.Count - 1

                Condition += String.Format("'{0}',", lvCategory.CheckedItems(I).Text)
            Next
            Condition = Condition.Substring(0, Condition.Length - 1)
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select (PID),(ProductCode), (Productname),(Department), (SubCategoryID),(CategoryName),(SubCategoryName), (Description),(UPCCode), (Price),(Discount),EntryDate from Category,SubCategory,Product,Department where Category.CategoryName=SubCategory.Category and Product.SubCategoryID=SubCategory.ID and Department.DepartmentName=Product.Department and CategoryName in (" & Condition & ")", con)
            adp = New SqlDataAdapter(cmd)
            dtable = New DataTable()
            adp.Fill(dtable)
            con.Close()
            dgw.DataSource = dtable
            ds = New DataSet()
            ds.Tables.Add(dtable)
            ds.WriteXmlSchema("ProductsReport.xml")
            Dim rpt As New rptProductReport
            rpt.SetDataSource(ds)
            frmReport.CrystalReportViewer1.ReportSource = rpt
            frmReport.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Try
            Cursor = Cursors.WaitCursor
            Timer1.Enabled = True
            Dim Condition As String = ""
            For I = 0 To lvSubCategory.CheckedItems.Count - 1

                Condition += String.Format("'{0}',", lvSubCategory.CheckedItems(I).Text)
            Next
            Condition = Condition.Substring(0, Condition.Length - 1)
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select (PID),(ProductCode), (Productname),(Department), (SubCategoryID),(CategoryName),(SubCategoryName), (Description),(UPCCode), (Price),(Discount),EntryDate from Category,SubCategory,Product,Department where Category.CategoryName=SubCategory.Category and Product.SubCategoryID=SubCategory.ID and Department.DepartmentName=Product.Department and SubCategoryName in (" & Condition & ")", con)
            adp = New SqlDataAdapter(cmd)
            dtable = New DataTable()
            adp.Fill(dtable)
            con.Close()
            dgw.DataSource = dtable
            ds = New DataSet()
            ds.Tables.Add(dtable)
            ds.WriteXmlSchema("ProductsReport.xml")
            Dim rpt As New rptProductReport
            rpt.SetDataSource(ds)
            frmReport.CrystalReportViewer1.ReportSource = rpt
            frmReport.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Cursor = Cursors.Default
        Timer1.Enabled = False
    End Sub

    Private Sub btnViewReport_Click(sender As System.Object, e As System.EventArgs) Handles btnViewReport.Click '
        Try
            Cursor = Cursors.WaitCursor
            Timer1.Enabled = True
            Dim Condition As String = ""
            For I = 0 To lvProductName.CheckedItems.Count - 1

                Condition += String.Format("'{0}',", lvProductName.CheckedItems(I).Text)
            Next
            Condition = Condition.Substring(0, Condition.Length - 1)
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select (PID),(ProductCode), (Productname),(Department), (SubCategoryID),(CategoryName),(SubCategoryName), (Description),(UPCCode),(Price),(Discount),EntryDate from Category,SubCategory,Product,Department where Category.CategoryName=SubCategory.Category and Product.SubCategoryID=SubCategory.ID and Department.DepartmentName=Product.Department and ProductName in (" & Condition & ")", con)
            adp = New SqlDataAdapter(cmd)
            dtable = New DataTable()
            adp.Fill(dtable)
            con.Close()
            dgw.DataSource = dtable
            ds = New DataSet()
            ds.Tables.Add(dtable)
            ds.WriteXmlSchema("ProductsReport.xml")
            Dim rpt As New rptProductReport
            rpt.SetDataSource(ds)
            frmReport.CrystalReportViewer1.ReportSource = rpt
            frmReport.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Sub reset()
        Fill()
    End Sub

    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click
        reset()
    End Sub
End Class
