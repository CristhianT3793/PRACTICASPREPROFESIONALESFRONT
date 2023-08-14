<%@ Page Title="Practicas Preprofesionales UISEK" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FPP_front._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            var cedula = '1716631286';
            console.log(cedula);
            console.log("paso 1");
            $.ajax({
                type: "POST",
                url: 'https://tutorias.uisek.edu.ec/WSValidarTutorias.asmx/validarTutorias',
                contentType: "application/json; charset=utf-8",
                data: "{cedula: '" + cedula + "'}",
                dataType: "json",

                success: function (response, status) {
                    var resp = response.d;

                    console.log(resp);
                    if (resp != "" && !resp.includes("No Existe")) {
                        console.log(resp[0]);
                    }
                    else {
                        console.log("La cédula ingresada no existe como alumno.");
                    }
                }, statusCode: {
                    404: function (content) { console.log('cannot find resource'); },
                    500: function (content) { console.log('internal server error'); }
                },
                error: function (xhr, status, error) {
                    console.log("Error ");
                }
            });
        });
    </script>
</asp:Content>
