Imports System.Data.SqlClient
Public Class frmMainMenu
    Dim Filename As String
    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        frmAbout.ShowDialog()
    End Sub

    Private Sub RegistrationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RegistrationToolStripMenuItem.Click
        frmRegistration.lblUser.Text = lblUser.Text
        frmRegistration.Reset()
        frmRegistration.ShowDialog()
    End Sub

    Private Sub DepartmentToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DepartmentToolStripMenuItem.Click
        frmDepartment.lblUser.Text = lblUser.Text
        frmDepartment.Reset()
        frmDepartment.ShowDialog()
    End Sub

    Private Sub CategoryToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CategoryToolStripMenuItem.Click
        frmCategory.lblUser.Text = lblUser.Text
        frmCategory.Reset()
        frmCategory.ShowDialog()
    End Sub

    Private Sub SubCategoryToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SubCategoryToolStripMenuItem.Click
        frmSubCategory.lblUser.Text = lblUser.Text
        frmSubCategory.Reset()
        frmSubCategory.ShowDialog()
    End Sub

    Private Sub ProductToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ProductToolStripMenuItem.Click
        frmProduct.lblUser.Text = lblUser.Text
        frmProduct.Reset()
        frmProduct.ShowDialog()
    End Sub

    Private Sub LogsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LogsToolStripMenuItem.Click
        frmLogs.Reset()
        frmLogs.lblUser.Text = lblUser.Text
        frmLogs.ShowDialog()
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LogoutToolStripMenuItem.Click
        Try
            If MessageBox.Show("Do you really want to logout from application?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                If MessageBox.Show("Do you want backup database before logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Backup()
                    LogOut()
                Else
                    LogOut()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RestoreToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RestoreToolStripMenuItem.Click
        Try
            With OpenFileDialog1
                .Filter = ("DB Backup File|*.bak;")
                .FilterIndex = 4
            End With
            'Clear the file name
            OpenFileDialog1.FileName = ""

            If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                Cursor = Cursors.WaitCursor
                Timer2.Enabled = True
                SqlConnection.ClearAllPools()
                con = New SqlConnection(cs)
                con.Open()
                Dim cb As String = "USE Master ALTER DATABASE [" & System.Windows.Forms.Application.StartupPath & "\PMS_DB.mdf] SET Single_User WITH Rollback Immediate Restore database [" & System.Windows.Forms.Application.StartupPath & "\PMS_DB.mdf] FROM disk='" & OpenFileDialog1.FileName & "' WITH REPLACE ALTER DATABASE [" & System.Windows.Forms.Application.StartupPath & "\PMS_DB.mdf] SET Multi_User "
                cmd = New SqlCommand(cb)
                cmd.Connection = con
                cmd.ExecuteReader()
                con.Close()
                Dim st As String = "Sucessfully performed the restore"
                LogFunc(lblUser.Text, st)
                MessageBox.Show("Successfully performed", "Database Restore", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Sub Backup()
        Try
            Dim destdir As String = "PMS_DB " & DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") & ".bak"
            Dim objdlg As New SaveFileDialog
            objdlg.FileName = destdir
            objdlg.ShowDialog()
            Filename = objdlg.FileName
            Cursor = Cursors.WaitCursor
            Timer2.Enabled = True
            con = New SqlConnection(cs)
            con.Open()
            Dim cb As String = "backup database [" & System.Windows.Forms.Application.StartupPath & "\PMS_DB.mdf] to disk='" & Filename & "'with init,stats=10"
            cmd = New SqlCommand(cb)
            cmd.Connection = con
            cmd.ExecuteReader()
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Sub LogOut()
        Me.Hide()
        frmLogin.Show()
        frmLogin.UserID.Text = ""
        frmLogin.Password.Text = ""
        frmLogin.UserID.Focus()
    End Sub
    Private Sub frmMainMenu_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Timer2_Tick(sender As System.Object, e As System.EventArgs) Handles Timer2.Tick
        Cursor = Cursors.Default
        Timer2.Enabled = False
    End Sub

    Private Sub BackupToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BackupToolStripMenuItem.Click
        Backup()
    End Sub

    Private Sub frmMainMenu_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        lblDateTime.Text = Now
    End Sub

    Private Sub NotepadToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NotepadToolStripMenuItem.Click
        System.Diagnostics.Process.Start("Notepad.exe")
    End Sub

    Private Sub CalculatorToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CalculatorToolStripMenuItem.Click
        System.Diagnostics.Process.Start("Calc.exe")
    End Sub

    Private Sub WordpadToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles WordpadToolStripMenuItem.Click
        System.Diagnostics.Process.Start("Wordpad.exe")
    End Sub

    Private Sub MSPaintToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MSPaintToolStripMenuItem.Click
        System.Diagnostics.Process.Start("MSpaint.exe")
    End Sub

    Private Sub MSWordToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MSWordToolStripMenuItem.Click
        System.Diagnostics.Process.Start("Winword.exe")
    End Sub

    Private Sub TaskManagerToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TaskManagerToolStripMenuItem.Click
        System.Diagnostics.Process.Start("TaskMgr.exe")
    End Sub

    Private Sub SystemInfoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SystemInfoToolStripMenuItem.Click
        frmSystemInfo.ShowDialog()
    End Sub

    Private Sub CompanyToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CompanyToolStripMenuItem.Click
        frmCompany.lblUser.Text = lblUser.Text
        frmCompany.Reset()
        frmCompany.ShowDialog()
    End Sub

    Private Sub PurchaseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PurchaseToolStripMenuItem.Click
        frmPurchase.lblUser.Text = lblUser.Text
        frmPurchase.Reset()
        frmPurchase.ShowDialog()
    End Sub

    Private Sub ProductsToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles ProductsToolStripMenuItem1.Click
        Dim st As String = "View products report"
        LogFunc(lblUser.Text, st)
        frmProductReport.reset()
        frmProductReport.ShowDialog()
    End Sub

    Private Sub ProductsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ProductsToolStripMenuItem.Click
        frmProductRecord.Reset()
        frmProductRecord.ShowDialog()
    End Sub

    Private Sub PurchasesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PurchasesToolStripMenuItem.Click
        frmPurchaseRecord.Reset()
        frmPurchaseRecord.ShowDialog()
    End Sub

    Private Sub PurchasesToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles PurchasesToolStripMenuItem1.Click
        Dim st As String = "View purchases report"
        LogFunc(lblUser.Text, st)
        frmPurchaseReport.Reset()
        frmPurchaseReport.ShowDialog()
    End Sub

    Private Sub Purchases2ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles Purchases2ToolStripMenuItem.Click
        Dim st As String = "View purchases report"
        LogFunc(lblUser.Text, st)
        frmPurchaseReport1.Reset()
        frmPurchaseReport1.ShowDialog()
    End Sub
End Class