window["cherwell"] = window.cherwell || {};
cherwell["challenge"] = cherwell.challenge || {};

cherwell.challenge["trianglesearch"] =
{
    self: this,

    uri: 'triangles',

    searchMode: 'ByKey',

    showErrorMessage: function (message) {
        var newHtml = '<div class="alert alert-danger ajax-alert"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Oops! </strong>' + message + '</div>'

        $('#alert-area').append(newHtml);
        $('.ajax-alert').last().hide().fadeIn(1000);
    },

    showInstructions: function (message) {
        var newHtml = '<div class="alert alert-info ajax-alert"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>' + message + '</div>'

        $('#alert-area').append(newHtml);
        $('.ajax-alert').last().hide().fadeIn(1000);
    },

    showInfo: function (message) {
        var newHtml = '<div class="alert alert-info ajax-alert"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>' + message + '</div>'

        $('#alert-area').append(newHtml);
        $('.ajax-alert').last().hide().fadeIn(1000).fadeOut(5000, function () { $(this).remove(); });
    },

    onChangeSearchMethod: function () {
        
        const verticesByKeyArea = document.getElementById("verticesByKeyArea");
        const keyByVerticesArea = document.getElementById("keyByVerticesArea");
        if (!$("#searchMethodSwitch").is(':checked')) {
            this.searchMode = 'ByKey';
            console.log("Now the search mode is ByKey");
            // BY KEY
            // this is search by key so make the VerticesFound div visible and the keyByVerticesArea invisible
            // TODO: refactor
            if (verticesByKeyArea.classList.contains('d-none')) {
                verticesByKeyArea.classList.remove('d-none');
            }
           
            if (!keyByVerticesArea.classList.contains('d-none')) {
                keyByVerticesArea.classList.add('d-none');
            }
        }
        else {
            // BY VERTICES
            // make the verticesByKeyArea div invisible and the keyByVerticesArea visible
            this.searchMode = 'ByVertices';
            console.log("Now the search mode is ByVertices");
            if (!verticesByKeyArea.classList.contains('d-none')) {
                verticesByKeyArea.classList.add('d-none');
            }
                       
            if (keyByVerticesArea.classList.contains('d-none')) {
                keyByVerticesArea.classList.remove('d-none');
            }
        }
        this.sensitizeSearchButton();
    },

    sensitizeSearchButton: function () {
        // any time the user changes the choice for triangle key in the ByKey mode,
        // or edits any of the vertices in ByVertices mode (on blur) in the ByVertices mode we enable/disable the Search button searchBtn
        const searchBtn = document.getElementById("searchBtn");
        if (this.searchMode === "ByKey") {
            // try to get the value of the keySelect select control
            let triangleKey = document.getElementById("keySelect").value;
            if (triangleKey === "Choose a triangle key...") {
                console.log("Still need to choose a triangle key to enable search");
                $("#searchBtn").prop("disabled", true);
            }
            else {
                console.log("Search IS enabled now");
                $("#searchBtn").prop("disabled", false);
            }
        }
        else if (this.searchMode === "ByVertices") {
            let v1x = $("#Vertex1X").val();
            let v1y = $("#Vertex1Y").val();
            let v2x = $("#Vertex2X").val();
            let v2y = $("#Vertex2Y").val();
            let v3x = $("#Vertex3X").val();
            let v3y = $("#Vertex3Y").val();
            // check if they are all filled in.
            if (v1x !== undefined && v1x.length > 0 &&
                v1y !== undefined && v1y.length > 0 &&
                v2x !== undefined && v2x.length > 0 &&
                v2y !== undefined && v2y.length > 0 &&
                v3x !== undefined && v3x.length > 0 &&
                v3y !== undefined && v3y.length > 0) {
                $("#searchBtn").prop("disabled", false);
                console.log("Search IS enabled now");
            }
            else {
                $("#searchBtn").prop("disabled", true);
                console.log("Search is disabled; missing some coordinates");
            }
        }
    },


    validateForm: function () {
        if (this.searchMode === "ByKey") {
            let triangleKey = document.getElementById("keySelect").value;
            if (triangleKey === "Choose a triangle key...") {
                this.showErrorMessage("Please choose a triangle key");
                return false;
            }
            return true;    // low bar this case
        }
        else {
            let v1x = $("#Vertex1X").val();
            let v1y = $("#Vertex1Y").val();
            let v2x = $("#Vertex2X").val();
            let v2y = $("#Vertex2Y").val();
            let v3x = $("#Vertex3X").val();
            let v3y = $("#Vertex3Y").val();
            if (v1x !== undefined && v1x.length > 0 &&
                v1y !== undefined && v1y.length > 0 &&
                v2x !== undefined && v2x.length > 0 &&
                v2y !== undefined && v2y.length > 0 &&
                v3x !== undefined && v3x.length > 0 &&
                v3y !== undefined && v3y.length > 0) {
                // todo: check for valid numbers and so on, range, 0-60
                return true;
            }
            else {
                this.showErrorMessage("Please fill in all the vertices.");
                return false;
            }
        }
    },

    displayVertices: function (oneTriangleDetails) {
        console.log(oneTriangleDetails);
        this.showInfo("Found triangle: " + oneTriangleDetails.triangleKey);
        $("#ReadonlyVertex1X").val(oneTriangleDetails.vertex1.x);
        $("#ReadonlyVertex1Y").val(oneTriangleDetails.vertex1.y);
        $("#ReadonlyVertex2X").val(oneTriangleDetails.vertex2.x);
        $("#ReadonlyVertex2Y").val(oneTriangleDetails.vertex2.y);
        $("#ReadonlyVertex3X").val(oneTriangleDetails.vertex3.x);
        $("#ReadonlyVertex3Y").val(oneTriangleDetails.vertex3.y);

    },

    displayFoundTriangleKey: function (oneTriangleDetails) {
        console.log(oneTriangleDetails);
        if (oneTriangleDetails.isValid) {
            this.showInfo("Found triangle: " + oneTriangleDetails.triangleKey);
            $("#foundKey").val(oneTriangleDetails.triangleKey);
        }
        else {
            this.showErrorMessage("The coordinates specified are not valid.");
        }
    },

    onSearch: function () {
        if (this.validateForm()) {
            if (this.searchMode === "ByKey") {
                let id = document.getElementById("keySelect").value;
                fetch(`${this.uri}/${id}`)
                    .then(response => response.json())
                    .then(data => this.displayVertices(data))
                    .catch(error => console.error('Unable to get items', error));
            }
            else if (this.searchMode === "ByVertices") {
                let v1x = $("#Vertex1X").val();
                let v1y = $("#Vertex1Y").val();
                let v2x = $("#Vertex2X").val();
                let v2y = $("#Vertex2Y").val();
                let v3x = $("#Vertex3X").val();
                let v3y = $("#Vertex3Y").val();
                let finalUrl = `${this.uri}/search/?v1=${v1x},${v1y}&v2=${v2x},${v2y}&v3=${v3x},${v3y}`;
                console.log("The finalUrl:" + finalUrl);
                fetch(`${this.uri}/search/?v1=${v1x},${v1y}&v2=${v2x},${v2y}&v3=${v3x},${v3y}`)
                    .then(response => response.json())
                    .then(data => this.displayFoundTriangleKey(data))
                    .catch(error => console.error('Unable to resolve triangle key from vertices', error));
            }
        }
    }
};

jQuery(document).ready(function () {
    console.log("Entering document ready for triangle search view");

    cherwell.challenge.trianglesearch.showInstructions("Choose the search method - by key or by vertices, then click the 'Search' button. When searching by vertices, the coordinates must be factors of 10 (0-60), and the set of vertices should define a triangle.");
    cherwell.challenge.trianglesearch.sensitizeSearchButton();


});
