//To be able to navigate from the top of the site
function ResolveUrl(url) {
    //var baseUrl = location.protocol + '//' + location.host
    if (url.indexOf("~/") == 0) {
        url = BaseUrl + url.substring(2);
    }
    return url;
}

function getXML(data) {
    if (data.xml)
        return data.xml;
    else {
        if (window.XMLSerializer) {
            try {
                var ser = new XMLSerializer();
                return ser.serializeToString(data);
            }
            catch (exp) {
                return null;
            }
        }
        else
            return null;
    }
}

function RequestQueryVariable(p_Param) {
    var pageUrl = window.location.search.substring(1);
    var urlVariables = pageUrl.split('&');
    for (var i = 0; i < urlVariables.length; i++) {
        var parameterName = urlVariables[i].split('=');
        if (parameterName[0] == p_Param) {
            return parameterName[1];
        }
    }
    return null;
}
