//alert("Connect");
        //cHANGE
            var CLIENTID = '983461715427-gucjfhfbokl8c3puieod2s5le69mbsh0.apps.googleusercontent.com';
            var REDIRECT = 'https://localhost:44363/Account/Register';
            var LOGOUT = 'https://localhost:44363/Account/Register';
            var Located_Func = '/Account/GoogleLogin/';
            //var After = "Login.cshtml";
        // 
  var OAUTHURL = 'https://accounts.google.com/o/oauth2/auth?';
        var VALIDURL = 'https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=';
        var SCOPE = 'https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email';
   
        
        var TYPE = 'token';
        var _url = OAUTHURL + 'scope=' + SCOPE + '&client_id=' + CLIENTID + '&redirect_uri=' + REDIRECT + '&response_type=' + TYPE;
        var acToken;
        var tokenType;
        var expiresIn;
        var user;
        var loggedIn = false;

        function login() {

            var win = window.open(_url, "windowname1", 'width=800, height=600');
            var pollTimer = window.setInterval(function () {
                try {
                    console.log(win.document.URL);
                    if (win.document.URL.indexOf(REDIRECT) != -1) {
                        window.clearInterval(pollTimer);
                        var url = win.document.URL;
                        acToken = gup(url, 'access_token');
                        tokenType = gup(url, 'token_type');
                        expiresIn = gup(url, 'expires_in');

                        win.close();
                        debugger;
                        validateToken(acToken);
                    }
                }
                catch (e) {

                }
            }, 500);
        }

        function gup(url, name) {
            namename = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\#&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(url);
            if (results == null)
                return "";
            else
                return results[1];
        }

        function validateToken(token) {

            getUserInfo();
            $.ajax(

                {

                    url: VALIDURL + token,
                    data: null,
                    success: function (responseText) {


                    },

                });

        }

        function getUserInfo() {

            var k;
            $.ajax({

                url: 'https://www.googleapis.com/oauth2/v1/userinfo?access_token=' + acToken,
                type: 'GET',
                data: null,
                success: function (resp) {
                    
                    user = resp;
                    SetInfo(user);
                   // console.log(user);
                    
                   // $('#uname').html('Welcome ' + user.name);
                   // $('#uemail').html('Email: ' + user.email)
                   // $('#imgHolder').attr('src', user.picture);
                    


                },


            })
               // alert(k);
           


        }
        function SetInfo() {
            $.ajax({

                url: Located_Func,
                type: 'POST',
                data: {
                    email: user.email,
                    name: user.name,
                    gender: user.gender,
                    lastname: user.lastname,
                    location: user.location
                },
                success: function (response) {     
                    window.location.href = response.redirectToUrl ;
                    
                }

                

            });
        }