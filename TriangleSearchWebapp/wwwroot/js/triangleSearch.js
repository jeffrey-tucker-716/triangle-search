window["cherwell"] = window.cherwell || {};
cherwell["challenge"] = cherwell.challenge || {};

cherwell.challenge["trianglesearch"] =
{
    self: this,

    searchMode: 'ByKey',

    onChangeSearchMethod: function () {
        
        const verticesByKeyArea = document.getElementById("verticesByKeyArea");
        const keyByVerticesArea = document.getElementById("keyByVerticesArea");
        if (!$("#searchMethodSwitch").is(':checked')) {
            searchMode = 'ByKey';
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
            searchMode = 'ByVertices';
            console.log("Now the search mode is ByVertices");
            if (!verticesByKeyArea.classList.contains('d-none')) {
                verticesByKeyArea.classList.add('d-none');
            }
                       
            if (keyByVerticesArea.classList.contains('d-none')) {
                keyByVerticesArea.classList.remove('d-none');
            }
        }
    }
};

jQuery(document).ready(function () {
    console.log("Entering document ready for triangle search view");





});
