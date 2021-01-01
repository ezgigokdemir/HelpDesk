var textHelper = {
    escapeSpecialChars(jsonString) {
        return jsonString
            //.replace(/\"/g, "")
            .replace(/\//g, "")
            .replace(/\\/g, "")
            .replace(/\b/g, "")
            .replace(/\f/g, "")
            .replace(/\n/g, "")
            .replace(/\r/g, "")
            .replace(/\t/g, "");
    }
}
