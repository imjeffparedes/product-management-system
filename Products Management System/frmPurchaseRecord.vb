Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO

Public Class frmPurchaseRecord

    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select PI_ID, Date, RTRIM(PurchaseType), RTRIM(Company), SubTotal, TaxPer, TaxAmount, GrandTotal, RTRIM(Remarks) from Purchase order by Date", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub frmLogs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Getdata()
    End Sub

    Private Sub dgw_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles dgw.MouseClick
        Try
            If dgw.Rows.Count > 0 Then
                If lblSet.Text = "Purchase" Then
                    Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                    frmPurchase.Show()
                    Me.Hide()
                    frmPurchase.txtID.Text = dr.Cells(0).Value.ToString()
                    frmPurchase.dtpDate.Text = dr.Cells(1).Value.ToString()
                    frmPurchase.cmbPurchaseType.Text = dr.Cells(2).Value.ToString()
                    frmPurchase.cmbCompany.Text = dr.Cells(3).Value.ToString()
                    frmPurchase.txtSubTotal.Text = dr.Cells(4).Value.ToString()
                    frmPurchase.txtTaxPer.Text = dr.Cells(5).Value.ToString()
                    frmPurchase.txtTaxAmount.Text = dr.Cells(6).Value.ToString()
                    frmPurchase.txtGrandTotal.Text = dr.Cells(7).Value.ToString()
                    frmPurchase.txtRemarks.Text = dr.Cells(8).Value.ToString()
                    frmPurchase.btnSave.Enabled = False
                    frmPurchase.btnUpdate.Enabled = True
                    frmPurchase.btnDelete.Enabled = True
                    con = New SqlConnection(cs)
                    con.Open()
                    Dim sql As String = "SELECT ProductID,RTRIM(Product.ProductCode),RTRIM(Productname),Quantity,Purchase_Product.Price,TotalAmount from Purchase,Purchase_Product,product where product.PID=Purchase_product.ProductID and Purchase.PI_ID=Purchase_Product.PurchaseID and PI_ID=" & dr.Cells(0).Value & ""
                    cmd = New SqlCommand(sql, con)
                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    frmPurchase.DataGridView1.Rows.Clear()
                    While (rdr.Read() = True)
                        frmPurchase.DataGridView1.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5))
                    End While
                    con.Close()
                    lblSet.Text = ""
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgw_RowPostPaint(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles dgw.RowPostPaint
        Dim strRowNumber As String = (e.RowIndex + 1).ToString()
        Dim size As SizeF = e.Graphics.MeasureString(strRowNumber, Me.Font)
        If dgw.RowHeadersWidth < Convert.ToInt32((size.Width + 20)) Then
            dgw.RowHeadersWidth = Convert.ToInt32((size.Width + 20))
        End If
        Dim b As Brush = SystemBrushes.ControlText
        e.Graphics.DrawString(strRowNumber, Me.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))

    End Sub
    Sub Reset()
        txtCompany.Text = ""
        cmbPurchaseType.SelectedIndex = -1
        dtpDateFrom.Text = Today
        DateTimePicker1.Text = Today
        DateTimePicker2.Text = Today
        dtpDateTo.Text = Today
        Getdata()
    End Sub
    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtSupplierName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCompany.TextChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select PI_ID, Date, RTRIM(PurchaseType), RTRIM(Company), SubTotal, TaxPer, TaxAmount, GrandTotal, RTRIM(Remarks) from Purchase where Company like '%" & txtCompany.Text & "%' order by Date", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub btnExportExcel_Click(sender As System.Object, e As System.EventArgs) Handles btnExportExcel.Click
        Dim rowsTotal, colsTotal As Short
        Dim I, j, iC As Short
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Dim xlApp As New Excel.Application
        Try
            Dim excelBook As Excel.Workbook = xlApp.Workbooks.Add
            Dim excelWorksheet As Excel.Worksheet = CType(excelBook.Worksheets(1), Excel.Worksheet)
            xlApp.Visible = True

            rowsTotal = dgw.RowCount
            colsTotal = dgw.Columns.Count - 1
            With excelWorksheet
                .Cells.Select()
                .Cells.Delete()
                For iC = 0 To colsTotal
                    .Cells(1, iC + 1).Value = dgw.Columns(iC).HeaderText
                Next
                For I = 0 To rowsTotal - 1
                    For j = 0 To colsTotal
                        .Cells(I + 2, j + 1).value = dgw.Rows(I).Cells(j).Value
                    Next j
                Next I
                .Rows("1:1").Font.FontStyle = "Bold"
                .Rows("1:1").Font.Size = 12

                .Cells.Columns.AutoFit()
                .Cells.Select()
                .Cells.EntireColumn.AutoFit()
                .Cells(1, 1).Select()
            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RELEASE ALLOACTED RESOURCES
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            xlApp = Nothing
        End Try
    End Sub

    Private Sub btnGetData_Click(sender As System.Object, e As System.EventArgs) Handles btnGetData.Click
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select PI_ID, Date, RTRIM(PurchaseType), RTRIM(Company), SubTotal, TaxPer, TaxAmount, GrandTotal, RTRIM(Remarks) from Purchase where Date between @d1 and @d2 order by Date", con)
            cmd.Parameters.Add("@d1", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(dtpDateFrom.Value.Date)
            cmd.Parameters.Add("@d2", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(dtpDateTo.Value.Date)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8))
            End While
            con.Close()
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
            cmd = New SqlCommand("Select PI_ID, Date, RTRIM(PurchaseType), RTRIM(Company), SubTotal, TaxPer, TaxAmount, GrandTotal, RTRIM(Remarks) from Purchase where Date between @d1 and @d2 and PurchaseType='" & cmbPurchaseType.Text & "' order by Date", con)
            cmd.Parameters.Add("@d1", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(DateTimePicker2.Value.Date)
            cmd.Parameters.Add("@d2", SqlDbType.Date, 30, "Date").Value = Convert.ToDateTime(DateTimePicker1.Value.Date)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
