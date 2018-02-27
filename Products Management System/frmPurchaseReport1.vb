Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO

Public Class frmPurchaseReport1
    Dim a, b, c, d, s, f, g As Decimal
    Sub Reset()
        dtpDateFrom.Text = Today
        dtpDateTo.Text = Today
    End Sub
    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
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
            Dim rpt As New rptPurchase2 'The report you created.
            Dim myConnection As SqlConnection
            Dim MyCommand As New SqlCommand()
            Dim myDA As New SqlDataAdapter()
            Dim myDS As New DataSet 'The DataSet you created.
            myConnection = New SqlConnection(cs)
            MyCommand.Connection = myConnection
            MyCommand.CommandText = "SELECT Purchase.PI_ID, Purchase.Date, Purchase.PurchaseType, Purchase.Company, Purchase.SubTotal, Purchase.TaxPer, Purchase.TaxAmount, Purchase.GrandTotal, Purchase.Remarks, Purchase_Product.PP_ID,Purchase_Product.ProductID, Purchase_Product.Price, Purchase_Product.Quantity, Purchase_Product.TotalAmount, Purchase_Product.PurchaseID, Product.PID, Product.ProductCode, Product.ProductName,Product.UPCCode, Product.SubCategoryID, Product.Department, Product.Description, Product.Price AS Expr1, Product.Discount, Product.EntryDate FROM Purchase INNER JOIN Purchase_Product ON Purchase.PI_ID = Purchase_Product.PurchaseID INNER JOIN Product ON Purchase_Product.ProductID = Product.PID where Date between @d1 and @d2 order by Date"
            MyCommand.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            MyCommand.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value.Date
            MyCommand.CommandType = CommandType.Text
            myDA.SelectCommand = MyCommand
            myDA.Fill(myDS, "Purchase")
            myDA.Fill(myDS, "Purchase_Product")
            myDA.Fill(myDS, "Product")
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select ISNULL(sum(SubTotal),0),ISNULL(sum(TaxAmount),0),ISNULL(sum(GrandTotal),0) from Purchase where Date between @d1 and @d2 and PurchaseType='Local'"
            cmd = New SqlCommand(ct)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value.Date
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                a = rdr.GetValue(0)
                c = rdr.GetValue(1)
                s = rdr.GetValue(2)
            Else
                a = 0
                c = 0
                s = 0
            End If
            con.Close()
            con = New SqlConnection(cs)
            con.Open()
            Dim ct1 As String = "select ISNULL(sum(SubTotal),0),ISNULL(sum(TaxAmount),0),ISNULL(sum(GrandTotal),0) from Purchase where Date between @d1 and @d2 and PurchaseType='Non Local'"
            cmd = New SqlCommand(ct1)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value.Date
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                b = rdr.GetValue(0)
                d = rdr.GetValue(1)
                f = rdr.GetValue(2)
            Else
                b = 0
                d = 0
                f = 0
            End If
            con.Close()
            con = New SqlConnection(cs)
            con.Open()
            Dim ct2 As String = "select ISNULL(sum(GrandTotal),0) from Purchase where Date between @d1 and @d2"
            cmd = New SqlCommand(ct2)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value.Date
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                g = rdr.GetValue(0)
            Else
                g = 0
            End If
            con.Close()
            rpt.SetDataSource(myDS)
            rpt.SetParameterValue("p1", dtpDateFrom.Value.Date)
            rpt.SetParameterValue("p2", dtpDateTo.Value.Date)
            rpt.SetParameterValue("p3", a)
            rpt.SetParameterValue("p4", b)
            rpt.SetParameterValue("p5", c)
            rpt.SetParameterValue("p6", d)
            rpt.SetParameterValue("p7", s)
            rpt.SetParameterValue("p8", f)
            rpt.SetParameterValue("p9", g)
            frmReport.CrystalReportViewer1.ReportSource = rpt
            frmReport.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
