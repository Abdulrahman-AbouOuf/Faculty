Imports System
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Drawing.Image
Imports MongoDB.Bson
Imports MongoDB.Bson.IO
Imports MongoDB.Bson.Serialization
Imports MongoDB.Driver
Imports MongoDB.Resources
Imports MongoDB.Driver.Linq
Imports MongoDB.Driver.Builders

Public Class Form1
    Dim client As MongoClient
    Dim db As IMongoDatabase
    Dim collection As IMongoCollection(Of BsonDocument)
    Dim dt As New DataTable
    Dim datasourceflag As Boolean

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        client = New MongoClient("mongodb://localhost:27017/")
        db = client.GetDatabase("db")
        collection = db.GetCollection(Of BsonDocument)("Models")
        dt.Columns.Add("id", Type.GetType("System.Int32"))
        dt.Columns.Add("Model Type", Type.GetType("System.String"))
        dt.Columns.Add("Image", Type.GetType("System.Byte[]"))
    End Sub

    Private Sub btn_add_Click(sender As Object, e As EventArgs) Handles btn_add.Click
        OpenFileDialog1.Filter = "Image Files |*.bmp;*.gif;*.jpg;*.png;*.tif|Allfiles|*.*"
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            PictureBox1.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName, True)
        End If

        Dim f = Builders(Of BsonDocument).Filter.Empty
        For Each cmp As BsonDocument In collection.Find(f).ToList
            Dim id As BsonElement = cmp.GetElement("id")
            If txt_mno.Text = id.Value Then
                MsgBox("This id is already taken")
                Exit Sub
            End If
        Next
        Dim memStr As MemoryStream = New MemoryStream()
        PictureBox1.BackgroundImage.Save(memStr, ImageFormat.Png)
        Dim byteArray As Byte() = memStr.ToArray()
        Dim doc As BsonDocument = New BsonDocument()
        With doc
            .Add("id", txt_mno.Text)
            .Add("model", txt_mtype.Text)
            .Add("image", byteArray)
        End With
        collection.InsertOne(doc)
    End Sub

    Private Sub btn_show_Click(sender As Object, e As EventArgs) Handles btn_show.Click
        Dim f = Builders(Of BsonDocument).Filter.Empty
        For Each cmp As BsonDocument In collection.Find(f).ToList
            Dim id As BsonElement = cmp.GetElement("id")
            Dim model As BsonElement = cmp.GetElement("model")
            Dim image As BsonElement = cmp.GetElement("image")
            Dim img As Byte() = image.Value
            Dim memStr As MemoryStream = New MemoryStream(img)
            If txt_mno.Text = id.Value Then
                txt_mtype.Text = model.Value
                PictureBox1.BackgroundImage = FromStream(memStr)
                Exit Sub
            End If
        Next
        MsgBox("No models found")
        txt_mno.Clear()
        txt_mtype.Clear()
        PictureBox1.BackgroundImage = Nothing
    End Sub

    Private Sub btn_showall_Click(sender As Object, e As EventArgs) Handles btn_showall.Click
        If datasourceflag = True Then DataGridView1.DataSource.clear()
        Dim f = Builders(Of BsonDocument).Filter.Empty
        For Each cmp As BsonDocument In collection.Find(f).ToList
            Dim id As BsonElement = cmp.GetElement("id")
            Dim model As BsonElement = cmp.GetElement("model")
            Dim image As BsonElement = cmp.GetElement("image")
            Dim img As Byte() = image.Value
            Dim memStr As MemoryStream = New MemoryStream(img)
            dt.Rows.Add(id.Value, model.Value, memStr.ToArray())
        Next
        DataGridView1.DataSource = dt
        datasourceflag = True
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        Dim f = Builders(Of BsonDocument).Filter.Empty
        For idx = 0 To dt.Rows.Count - 1
            Dim cmp As BsonDocument = (collection.Find(f).ToList)(idx)
            Dim id As BsonElement = cmp.GetElement("id")
            Dim model As BsonElement = cmp.GetElement("model")
            Dim image As BsonElement = cmp.GetElement("image")
            Dim img As Byte() = image.Value
            Dim memStr As MemoryStream = New MemoryStream(img)
            If txt_mno.Text = id.Value Then
                Dim val = Builders(Of BsonDocument).Filter.Eq(Of String)("id",
txt_mno.Text)
                collection.DeleteOne(val)
                Exit For
            End If
        Next
        txt_mno.Clear()
        txt_mtype.Clear()
        PictureBox1.BackgroundImage = Nothing
        DataGridView1.DataSource = dt
        datasourceflag = True
    End Sub

    Private Sub btn_edit_Click(sender As Object, e As EventArgs) Handles btn_edit.Click
        OpenFileDialog1.Filter = "Image Files |*.bmp;*.gif;*.jpg;*.png;*.tif|Allfiles|*.*"
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            PictureBox1.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName, True)
        End If

        Dim f = Builders(Of BsonDocument).Filter.Empty
        For Each cmp As BsonDocument In collection.Find(f).ToList
            Dim id As BsonElement = cmp.GetElement("id")
            If txt_mno.Text = id.Value Then
                Dim memStr As MemoryStream = New MemoryStream()
                PictureBox1.BackgroundImage.Save(memStr, ImageFormat.Png)
                Dim byteArray As Byte() = memStr.ToArray()
                'Dim doc As BsonDocument = New BsonDocument()
                Dim doc As BsonDocument = New BsonDocument()
                With doc
                    .Set("id", txt_mno.Text)
                    .Set("model", txt_mtype.Text)
                    .Set("image", byteArray)
                End With
                collection.InsertOne(doc)
            End If
        Next

    End Sub
End Class
