Imports System.Data.SqlClient
Imports System.Drawing.Imaging
Imports System.IO
Imports System.IO.Ports

Public Class Helper
    Public Shared ReadOnly Property ConnectionString As String
        Get
            Return "Server=localhost;Database=face_recognition_system_db;User Id=sa;Password=123456"
        End Get
    End Property

    Public Shared Function executeNonQuery(ByVal _cmd As SqlCommand) As Integer
        Using con As SqlConnection = New SqlConnection(ConnectionString)
            Dim rslt As Long = 0
            Dim cmd As SqlCommand = _cmd
            Dim trans As SqlTransaction = Nothing

            Try

                cmd.CommandTimeout = 0
                con.Open()
                cmd.CommandType = CommandType.Text
                cmd.Connection = con
                trans = con.BeginTransaction()
                cmd.Transaction = trans

                Try

                    rslt = Convert.ToInt64(cmd.ExecuteScalar)

                    trans.Commit()
                Catch ex As Exception
                    rslt = 0
                    trans.Rollback()
                    Throw ex
                End Try

                Return rslt
            Finally
                trans = Nothing
            End Try
        End Using
    End Function

    Public Shared Function executeNonQueryBool(ByVal _cmd As SqlCommand) As Boolean
        Using con As SqlConnection = New SqlConnection(ConnectionString)
            Dim rslt As Boolean = False
            Dim cmd As SqlCommand = _cmd
            Dim trans As SqlTransaction = Nothing

            Try
                cmd.CommandTimeout = 0
                con.Open()
                cmd.CommandType = CommandType.Text
                cmd.Connection = con
                trans = con.BeginTransaction()
                cmd.Transaction = trans

                Try

                    If cmd.ExecuteNonQuery() >= 1 Then
                        rslt = True
                    End If

                    trans.Commit()
                Catch ex As Exception
                    rslt = False
                    trans.Rollback()
                    Throw ex
                End Try

                Return rslt
            Finally
                trans = Nothing
            End Try
        End Using
    End Function

    Public Shared Function executeQuery(ByVal _cmd As SqlCommand) As System.Data.DataTable
        Using con As SqlConnection = New SqlConnection(ConnectionString)
            Dim dt As DataTable = New DataTable()

            Try
                con.Open()
                Dim cmd As SqlCommand = _cmd
                cmd.CommandType = CommandType.Text
                cmd.Connection = con
                dt.Load(cmd.ExecuteReader())
                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Function

    Public Shared Sub ChangePanelDisplay(ByVal frm As Form, ByVal pnl As Panel)
        pnl.Controls.Clear()
        frm.TopLevel = False
        pnl.Controls.Add(frm)
        frm.Dock = DockStyle.Fill
        frm.Show()
    End Sub

    Public Shared Sub DGVRenameColumns(ByVal dgv As DataGridView, ParamArray s As String())
        Dim i As Integer = 0

        For Each d As String In s
            dgv.Columns(i).HeaderCell.Value = d
            i += 1
        Next
    End Sub

    Public Shared Sub DGVHiddenColumns(ByVal dgv As DataGridView, ParamArray i As String())
        For Each d As String In i
            dgv.Columns(d).Visible = False
        Next
    End Sub

    Public Shared Sub DGVFillWeights(ByVal dgv As DataGridView, ByVal x As Object(), ByVal y As Integer())
        Dim m As Integer = 0

        For Each x1 As Integer In x
            dgv.Columns(x1).FillWeight = y(m)
            m += 1
        Next
    End Sub

    Public Shared Sub LoadCB(ByVal cb As ComboBox, ByVal dt As DataTable, ByVal dm As String, ByVal vm As String)
        cb.DataSource = dt
        cb.DisplayMember = dm
        cb.ValueMember = vm
    End Sub

    Public Shared Function CheckRequiredTXT(ParamArray i As TextBox()) As Boolean
        Dim bol As Boolean = True

        For Each d As TextBox In i
            If d.Text.Equals("") Then bol = False
        Next

        Return bol
    End Function

    Public Shared Function CheckRequiredDTP(ParamArray i As DateTimePicker()) As Boolean
        Dim bol As Boolean = True

        For Each d As DateTimePicker In i
            If d.Text.Equals("") Then bol = False
        Next

        Return bol
    End Function

    Public Shared Function CheckRequiredCB(ParamArray i As ComboBox()) As Boolean
        Dim bol As Boolean = True

        For Each d As ComboBox In i
            If d.Text.Equals("") Then bol = False
        Next

        Return bol
    End Function

    Public Shared Sub ClearTXT(ParamArray i As TextBox())
        For Each d As TextBox In i
            d.ResetText()
        Next
    End Sub

    Public Shared Sub ClearDTP(ParamArray i As DateTimePicker())
        For Each d As DateTimePicker In i
            d.ResetText()
        Next
    End Sub

    Public Shared Sub ClearDTPTimeOnly(ParamArray i As DateTimePicker())
        For Each d As DateTimePicker In i
            d.Text = "12:00 AM"
        Next
    End Sub

    Public Shared Sub ClearCB(ParamArray i As ComboBox())
        For Each d As ComboBox In i
            d.ResetText()
            d.SelectedIndex = -1
        Next
    End Sub

    Public Shared Sub ClearCHX(ParamArray i As CheckBox())
        For Each d As CheckBox In i
            d.Checked = False
        Next
    End Sub

    Public Shared Sub ClearRB(ParamArray i As RadioButton())
        For Each d As RadioButton In i
            d.Checked = False
        Next
    End Sub


    Public Shared Function ConverteByteArrayToImage(ByVal byteArrayIn As Byte()) As Bitmap
        Using mStream As MemoryStream = New MemoryStream(byteArrayIn)
            Dim bitmapp As Bitmap = New Bitmap(Image.FromStream(mStream))
            Return bitmapp
        End Using
    End Function

    Public Shared Function ConvertImageToByteArray(ByVal img As Image) As Byte()
        Dim converter As New ImageConverter
        Return converter.ConvertTo(img, GetType(Byte()))
    End Function


    Public Shared Function ConvertImageToString(ByVal x As Image) As String

        Dim bitmapp As Bitmap = New Bitmap(100, 100)

        Using g As Graphics = Graphics.FromImage(bitmapp)
            g.DrawImage(x, 0, 0, 100 + 1, 100 + 1)
        End Using

        Dim _imageConverter As ImageConverter = New ImageConverter()
        Dim xByte As Byte() = CType(_imageConverter.ConvertTo(bitmapp, GetType(Byte())), Byte())
        Return Convert.ToBase64String(xByte)

    End Function

    Public Shared Function IsImage(ByVal img As Image) As Boolean

        Dim bol As Boolean = False

        If img.RawFormat.Equals(ImageFormat.Jpeg) Then
            bol = True
        ElseIf img.RawFormat.Equals(ImageFormat.Bmp) Then
            bol = True
        ElseIf img.RawFormat.Equals(ImageFormat.Emf) Then
            bol = True
        ElseIf img.RawFormat.Equals(ImageFormat.Exif) Then
            bol = True
        ElseIf img.RawFormat.Equals(ImageFormat.Gif) Then
            bol = True
        ElseIf img.RawFormat.Equals(ImageFormat.Icon) Then
            bol = True
        ElseIf img.RawFormat.Equals(ImageFormat.Png) Then
            bol = True
        ElseIf img.RawFormat.Equals(ImageFormat.Tiff) Then
            bol = True
        ElseIf img.RawFormat.Equals(ImageFormat.Wmf) Then
            bol = True
        End If

        Return bol

    End Function


    Public Shared Function EncodeBase64(input As String) As String
        Return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input))
    End Function

    Public Shared Function DecodeBase64(input As String) As String
        Return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(input))
    End Function

    Public Shared Function SendSMS(com As String, num As String, msg As String) As Boolean

        Dim SerialPort As New System.IO.Ports.SerialPort()
        Dim bol As Boolean

        If SerialPort.IsOpen Then
            SerialPort.Close()
        End If

        SerialPort.PortName = com
        SerialPort.BaudRate = 9600
        SerialPort.Parity = Parity.None
        SerialPort.StopBits = StopBits.One
        SerialPort.DataBits = 8
        SerialPort.Handshake = Handshake.RequestToSend
        SerialPort.DtrEnable = True
        SerialPort.RtsEnable = True
        SerialPort.NewLine = vbCrLf

        Dim message As String
        message = msg

        Try
            SerialPort.Open()
        Catch ex As Exception
            bol = False
        End Try

        If SerialPort.IsOpen() Then
            SerialPort.Write("AT" & vbCrLf)
            SerialPort.Write("AT+CMGF=1" & vbCrLf)
            SerialPort.Write("AT+CMGS=" & Chr(34) & num & Chr(34) & vbCrLf)
            SerialPort.Write(message & Chr(26))

            If SerialPort.ReadExisting.Contains("OK") Then
                bol = True
            Else
                bol = False
            End If

            SerialPort.Close()

        End If

        Return bol

    End Function

    Public Shared Function DeleteSMS(com As String) As Boolean
        Dim SerialPort As New System.IO.Ports.SerialPort()
        Dim bol As Boolean

        If SerialPort.IsOpen Then
            SerialPort.Close()
        End If

        SerialPort.PortName = com
        SerialPort.BaudRate = 9600
        SerialPort.Parity = Parity.None
        SerialPort.StopBits = StopBits.One
        SerialPort.DataBits = 8
        SerialPort.Handshake = Handshake.RequestToSend
        SerialPort.DtrEnable = True
        SerialPort.RtsEnable = True
        SerialPort.NewLine = vbCrLf


        Try
            SerialPort.Open()
        Catch ex As Exception
            bol = False
        End Try

        If SerialPort.IsOpen() Then
            SerialPort.Write("AT+CMGD=1,4" & vbCrLf)
            MsgBox(SerialPort.ReadLine)

            SerialPort.Close()

        End If
    End Function


End Class

