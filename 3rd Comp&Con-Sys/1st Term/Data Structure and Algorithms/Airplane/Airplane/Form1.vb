Public Structure Booking
    Dim TripNo As Integer
    Dim TripDate As String
    Dim DepTime As String
    Dim ArriveTime As String
    Dim From As String
    Dim Desti As String

    <VBFixedArray(68)> Dim Seat() As Integer
    <VBFixedArray(68)> Dim name() As String
    <VBFixedArray(68)> Dim mail() As String
    <VBFixedArray(68)> Dim flightclass() As String
End Structure

Public Class Form1
    Dim Buttons(68) As Button
    Public FlightBook As Booking
    Dim position, CurrSeat, SeatFlag As Integer

    Private Sub btn_newflight_Click(sender As Object, e As EventArgs) Handles btn_newflight.Click
        If txt_no.Text = "" Or txt_arriv.Text = "" Or txt_date.Text = "" Or txt_dest.Text = "" Or txt_from.Text = "" Or txt_depa.Text = "" Then
            MsgBox("Missing data about the Flight Info")
            Exit Sub
        End If
        position = Loc(1) 'Specifies the current pos in a file
        Seek(1, Val(txt_no.Text)) 'take the file number and the position

        FlightBook.TripNo = Val(txt_no.Text)
        FlightBook.TripDate = txt_date.Text
        FlightBook.ArriveTime = txt_arriv.Text
        FlightBook.DepTime = txt_depa.Text
        FlightBook.From = txt_from.Text
        FlightBook.Desti = txt_dest.Text

        For i As Integer = 0 To 67 '68 seats in the flightbook array from 0 to 67 of the passengers
            FlightBook.Seat(i) = 0
            FlightBook.name(i) = "0"
            FlightBook.mail(i) = "0"
            FlightBook.flightclass(i) = "0"
        Next

        Dim cur As Integer = Val(txt_no.Text)
        FilePut(1, FlightBook, cur)
        ClearFlightInfo()
    End Sub

    Private Sub btn_book_Click(sender As Object, e As EventArgs) Handles btn_book.Click
        If txt_name.Text = "" Or txt_mail.Text = "" Or txt_class.Text = "" Then
            MsgBox("Please Fill your data and choose a seat")
            Exit Sub
        End If
        View()
        Dim result As DialogResult
        result = MessageBox.Show("Do you want to book this seat?", "Seat Booking", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

        If result = DialogResult.OK Then
            Buttons(CurrSeat).BackColor = Color.Orange
            SeatFlag = 1
            FileGet(1, FlightBook, Val(txt_no.Text))
            FlightBook.Seat(CurrSeat) = SeatFlag
            FlightBook.name(CurrSeat) = txt_name.Text
            FlightBook.mail(CurrSeat) = txt_mail.Text
            FlightBook.flightclass(CurrSeat) = txt_class.Text

            Dim cur As Integer = Val(txt_no.Text)
            FilePut(1, FlightBook, cur) 'add new passenger and update
        ElseIf result = DialogResult.Cancel Then
            Buttons(CurrSeat).BackColor = Color.Lime
            Exit Sub
        End If
        ClearPassengerInfo()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label10.Text = DateTime.Now.ToString()
        FileOpen(1, "flightdb.txt", OpenMode.Random, OpenAccess.ReadWrite, OpenShare.Shared, Len(FlightBook))
        FlightBook.Seat = New Integer(68) {}
        FlightBook.name = New String(68) {}
        FlightBook.mail = New String(68) {}
        FlightBook.flightclass = New String(68) {}

        For i As Integer = 1 To 68
            Buttons(i - 1) = Me.Controls("Button" & i)
            AddHandler Buttons(i - 1).Click, AddressOf SeatNo
        Next
    End Sub

    Private Sub btn_confirm_Click(sender As Object, e As EventArgs) Handles btn_confirm.Click
        View()

        If SeatFlag = 0 Then
            MessageBox.Show("This seat is not booked", "Booking Confirm", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf SeatFlag = 1 Then
            Dim result As DialogResult
            result = MessageBox.Show("Confirm booking this seat?", "Booking Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

            If result = DialogResult.OK Then
                Buttons(CurrSeat).BackColor = Color.Red
                SeatFlag = 2
                FileGet(1, FlightBook, Val(txt_no.Text))

                FlightBook.Seat(CurrSeat) = SeatFlag
                Dim cur As Integer = Val(txt_no.Text)
                FilePut(1, FlightBook, cur) 'update after confirming
            ElseIf result = DialogResult.Cancel Then
                Buttons(CurrSeat).BackColor = Color.Orange
                Exit Sub
            End If
        End If
        ClearPassengerInfo()
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        View()
        If SeatFlag = 1 Then
            Dim result As DialogResult
            result = MessageBox.Show("Booking is not confirmed yet, Do you want to cancel?", "Cancel Booking", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

            If result = DialogResult.OK Then
                Buttons(CurrSeat).BackColor = Color.Lime
                SeatFlag = 0

                FlightBook.Seat(CurrSeat) = 0
                FlightBook.name(CurrSeat) = "0"
                FlightBook.mail(CurrSeat) = "0"
                FlightBook.flightclass(CurrSeat) = "0"
            ElseIf result = DialogResult.Cancel Then
                Buttons(CurrSeat).BackColor = Color.Orange
                Exit Sub
            End If
        ElseIf SeatFlag = 2 Then
            MessageBox.Show("You already Confirmed booking this seat, You can't cancel your booking", "Error Canceling Booking", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        ClearPassengerInfo()
    End Sub

    Public Sub SeatNo(sender As Object, e As EventArgs)
        FileGet(1, FlightBook, Val(txt_no.Text))
        For i As Integer = 0 To Buttons.Length - 1
            If Buttons(i) Is sender Then
                CurrSeat = i
                Exit For
            End If
        Next

        SeatFlag = FlightBook.Seat(CurrSeat)
        If SeatFlag <> 0 Then
            txt_name.Text = FlightBook.name(CurrSeat)
            txt_mail.Text = FlightBook.mail(CurrSeat)
            txt_class.Text = FlightBook.flightclass(CurrSeat)
        End If
    End Sub
    Public Sub View()
        FileGet(1, FlightBook, Val(txt_no.Text))

        If FlightBook.TripNo <> Val(txt_no.Text) Then
            MessageBox.Show("We don't have this Flight", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            FileClose(1)
            Exit Sub
        End If
        txt_date.Text = FlightBook.TripDate
        txt_from.Text = FlightBook.From
        txt_dest.Text = FlightBook.Desti
        txt_arriv.Text = FlightBook.ArriveTime
        txt_depa.Text = FlightBook.DepTime

        For i As Integer = 0 To 67
            If FlightBook.Seat(i) = 1 Then
                Buttons(i).BackColor = Color.Orange
            ElseIf FlightBook.Seat(i) = 2 Then
                Buttons(i).BackColor = Color.Red
            Else
                Buttons(i).BackColor = Color.Lime
            End If
        Next
    End Sub

    Private Sub btn_load_Click(sender As Object, e As EventArgs) Handles btn_load.Click
        View()
    End Sub

    Private Sub btn_exit_Click(sender As Object, e As EventArgs) Handles btn_exit.Click
        End
    End Sub

    Public Sub ClearFlightInfo()
        txt_no.Clear()
        txt_from.Clear()
        txt_dest.Clear()
        txt_depa.Clear()
        txt_date.Clear()
        txt_arriv.Clear()
    End Sub

    Public Sub ClearPassengerInfo()
        txt_class.Clear()
        txt_mail.Clear()
        txt_name.Clear()
    End Sub

End Class
