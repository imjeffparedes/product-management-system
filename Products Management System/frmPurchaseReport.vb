Imports System.Data.SqlClient

Imports System.IO

Public Class frmPurchaseReport
    Dim a, b, c As Decimal
    Sub Reset()
        cmbCompany.Text = ""
        cmbPurchaseType.SelectedIndex = -1
        dtpDateFrom.Text = Today
        DateTimePicker1.Text = Today
        DateTimePicker2.Text = Today
        dtpDateTo.Text = Today
    End Sub
    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnViewReport_Click(sender As System.Object, e As System.EventArgs) Handles btnViewReport.Click
        Try
            If Len(Trim(cmbCompany.Text)) = 0 Then
                MessageBox.Show("Please select company", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbCompany.Focus()
                Exit Sub
            End If
            Cursor = Cursors.WaitCursor
            Timer1.Enabled = True
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT Product.PID, Product.ProductCode, Product.ProductName,Purchase_Product.PP_ID, Purchase_Product.ProductID, Purchase_Product.Price, Purchase_Product.Quantity, Purchase_Product.TotalAmount, Purchase_Product.PurchaseID, Purchase.PI_ID,Purchase.Date, Purchase.PurchaseType, Purchase.Company, Purchase.SubTotal, Purchase.TaxPer, Purchase.TaxAmount, Purchase.GrandTotal, Purchase.Remarks FROM Product INNER JOIN Purchase_Product ON Product.PID = Purchase_Product.ProductID INNER JOIN Purchase ON Purchase_Product.PurchaseID = Purchase.PI_ID where Company=@d1 order by Company", con)
            cmd.Parameters.AddWithValue("@d1", cmbCompany.Text)
            adp = New SqlDataAdapter(cmd)
            dtable = New DataTable()
            adp.Fill(dtable)
            con.Close()
            dgw.DataSource = dtable
            ds = New DataSet()
            ds.Tables.Add(dtable)
            ds.WriteXmlSchema("PurchasesReport.xml")
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select sum(SubTotal),Sum(TaxAmount),Sum(GrandTotal) from Purchase where Company=@d1"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", cmbCompany.Text)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            While rdr.Read()
                a = rdr.GetValue(0)
                b = rdr.GetValue(1)
                c = rdr.GetValue(2)
            End While
            Dim rpt As New rptPurchase1
            rpt.SetDataSource(ds)
            rpt.SetParameterValue("p3", a)
            rpt.SetParameterValue("p4", b)
            rpt.SetParameterValue("p5", c)
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

    Private Sub btnGetData_Click(sender As System.Object, e As System.EventArgs) Handles btnGetData.Click
        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim ctn As String = "select * from Purchase where Date between @d1 and @d2"
            cmd = New SqlCommand(ctn)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value.Date
            cmd.Connection = con
            rdr = cmd.ExecuteReader()

            If Not rdr.Read() Then
                MessageBox.Show("Sorry..No record found", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If
            Cursor = Cursors.WaitCursor
            Timer1.Enabled = True
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT Product.PID, Product.ProductCode, Product.ProductName,Purchase_Product.PP_ID, Purchase_Product.ProductID, Purchase_Product.Price, Purchase_Product.Quantity, Purchase_Product.TotalAmount, Purchase_Product.PurchaseID, Purchase.PI_ID,Purchase.Date, Purchase.PurchaseType, Purchase.Company, Purchase.SubTotal, Purchase.TaxPer, Purchase.TaxAmount, Purchase.GrandTotal, Purchase.Remarks FROM Product INNER JOIN Purchase_Product ON Product.PID = Purchase_Product.ProductID INNER JOIN Purchase ON Purchase_Product.PurchaseID = Purchase.PI_ID where date between @d1 and @d2 order by Date", con)
            cmd.Parameters.Add("@d1", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(dtpDateFrom.Value.Date)
            cmd.Parameters.Add("@d2", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(dtpDateTo.Value.Date)
            adp = New SqlDataAdapter(cmd)
            dtable = New DataTable()
            adp.Fill(dtable)
            con.Close()
            dgw.DataSource = dtable
            ds = New DataSet()
            ds.Tables.Add(dtable)
            ds.WriteXmlSchema("PurchasesReport.xml")
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select sum(SubTotal),Sum(TaxAmount),Sum(GrandTotal) from Purchase where Date between @d1 and @d2"
            cmd = New SqlCommand(ct)
            cmd.Parameters.Add("@d1", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(dtpDateFrom.Value.Date)
            cmd.Parameters.Add("@d2", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(dtpDateTo.Value.Date)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            While rdr.Read()
                a = rdr.GetValue(0)
                b = rdr.GetValue(1)
                c = rdr.GetValue(2)
            End While
            Dim rpt As New rptPurchase
            rpt.SetDataSource(ds)
            rpt.SetParameterValue("p1", dtpDateFrom.Value.Date)
            rpt.SetParameterValue("p2", dtpDateTo.Value.Date)
            rpt.SetParameterValue("p3", a)
            rpt.SetParameterValue("p4", b)
            rpt.SetParameterValue("p5", c)
            frmReport.CrystalReportViewer1.ReportSource = rpt
            frmReport.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            If Len(Trim(cmbPurchaseType.Text)) = 0 Then
                MessageBox.Show("Please select purchase type", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbPurchaseType.Focus()
                Exit Sub
            End If
            con = New SqlConnection(cs)
            con.Open()
            Dim ctn As String = "select * from Purchase where Date between @d1 and @d2 and PurchaseType='" & cmbPurchaseType.Text & "'"
            cmd = New SqlCommand(ctn)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = DateTimePicker2.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = DateTimePicker1.Value.Date
            cmd.Connection = con
            rdr = cmd.ExecuteReader()

            If Not rdr.Read() Then
                MessageBox.Show("Sorry..No record found", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If
            Cursor = Cursors.WaitCursor
            Timer1.Enabled = True
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT Product.PID, Product.ProductCode, Product.ProductName,Purchase_Product.PP_ID, Purchase_Product.ProductID, Purchase_Product.Price, Purchase_Product.Quantity, Purchase_Product.TotalAmount, Purchase_Product.PurchaseID, Purchase.PI_ID,Purchase.Date, Purchase.PurchaseType, Purchase.Company, Purchase.SubTotal, Purchase.TaxPer, Purchase.TaxAmount, Purchase.GrandTotal, Purchase.Remarks FROM Product INNER JOIN Purchase_Product ON Product.PID = Purchase_Product.ProductID INNER JOIN Purchase ON Purchase_Product.PurchaseID = Purchase.PI_ID where date between @d1 and @d2 and PurchaseType='" & cmbPurchaseType.Text & "' order by Date", con)
            cmd.Parameters.Add("@d1", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(DateTimePicker2.Value.Date)
            cmd.Parameters.Add("@d2", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(DateTimePicker1.Value.Date)
            adp = New SqlDataAdapter(cmd)
            dtable = New DataTable()
            adp.Fill(dtable)
            con.Close()
            dgw.DataSource = dtable
            ds = New DataSet()
            ds.Tables.Add(dtable)
            ds.WriteXmlSchema("PurchasesReport.xml")
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select sum(SubTotal),Sum(TaxAmount),Sum(GrandTotal) from Purchase where Date between @d1 and @d2 and PurchaseType='" & cmbPurchaseType.Text & "'"
            cmd = New SqlCommand(ct)
            cmd.Parameters.Add("@d1", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(DateTimePicker2.Value.Date)
            cmd.Parameters.Add("@d2", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(DateTimePicker1.Value.Date)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            While rdr.Read()
                a = rdr.GetValue(0)
                b = rdr.GetValue(1)
                c = rdr.GetValue(2)
            End While
            Dim rpt As New rptPurchase
            rpt.SetDataSource(ds)
            rpt.SetParameterValue("p1", DateTimePicker2.Value.Date)
            rpt.SetParameterValue("p2", DateTimePicker1.Value.Date)
            rpt.SetParameterValue("p3", a)
            rpt.SetParameterValue("p4", b)
            rpt.SetParameterValue("p5", c)
            frmReport.CrystalReportViewer1.ReportSource = rpt
            frmReport.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Sub fillCompany()
        Try
            con = New SqlConnection(cs)
            con.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(CompanyName) FROM Company order by 1", con)
            ds = New DataSet("ds")
            adp.Fill(ds)
            dtable = ds.Tables(0)
            cmbCompany.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbCompany.Items.Add(drow(0).ToString())
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub frmPurchaseReport_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        fillCompany()
    End Sub

    Private Sub cmbCompany_Format(sender As System.Object, e As System.Windows.Forms.ListControlConvertEventArgs) Handles cmbCompany.Format
        If (e.DesiredType Is GetType(String)) Then
            e.Value = e.Value.ToString.Trim
        End If
    End Sub
End Class
