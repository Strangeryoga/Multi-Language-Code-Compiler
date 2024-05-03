<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compiler.aspx.cs" Inherits="Compiler.Compiler" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Code Compiler</title>
    <!-- Bootstrap CSS -->
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-JcKb8q3iqJ61gNV9KGb8thSsNjpSL0n8PARn9HuZOnIxN0hoP+VmmDGMN5t9UJ0Z" crossorigin="anonymous">
    <style>
    
       </style>

</head>


<body>
    <h1 class="text-center mt-5">Compiler</h1>

    
    <form id="form1" runat="server" class="container mt-5">
        <div>
            <asp:Button ID="csharpButton" runat="server" Text="C#" OnClick="csharpButton_Click" CssClass="btn btn-primary mr-2" />
            <asp:Button ID="javaButton" runat="server" Text="Java" OnClick="javaButton_Click" CssClass="btn btn-primary" />
            <asp:Button ID="javascriptButton" runat="server" Text="JavaScript" OnClick="javascriptButton_Click" CssClass="btn btn-primary" /> <!-- New JavaScript button -->
            <asp:TextBox ID="codeTextBox" runat="server" TextMode="MultiLine" CssClass="form-control no-select" Rows="15" placeholder="Enter code..." ValidateRequestMode="Disabled"></asp:TextBox>
            <br />
            <asp:Button ID="compileButton" runat="server" Text="Run" OnClick="compileButton_Click" CssClass="btn btn-primary" />
           
            <br />
            <asp:Label ID="resultLabel" runat="server" Text="" CssClass="mt-3"></asp:Label>
            <br />

        </div>
    </form>

    <div class="container mt-5">
        <hr />
        <footer>
            
        </footer>
    </div>

    <!-- Bootstrap JS (optional, if you need Bootstrap JavaScript components) -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js" integrity="sha384-pzjw8f+xcBq5E5rXs3tn6cWV6XpoqKtfmG5LuzytM6gt/sM+76PVCmYlGD9wB6Vw" crossorigin="anonymous"></script>
    
    <!-- JavaScript to close the tab when switched away -->
     <script>
         // Start the timer when the page loads
        

     </script>
</body>
</html>