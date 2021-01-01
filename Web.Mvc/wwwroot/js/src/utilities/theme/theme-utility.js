var themeUtility = {
    showLoadingBar: function () {
        document.querySelector("#page-side-loader").style.display = "inline-block";
        //document.querySelectorAll("button").forEach(element => element.disabled = true);
    },

    hideLoadingBar: function () {
        document.querySelector("#page-side-loader").style.display = "none";
        //document.querySelectorAll("button").forEach(element => element.disabled = false);
    }
};

export { themeUtility };
