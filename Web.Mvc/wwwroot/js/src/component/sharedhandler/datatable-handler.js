var tableHandler = {
    handleResponse: function (self, response, pageNumber, pageLength) {
        self.tableConfig = response.data;
        self.tableConfig.pages = [];
        self.tableConfig.currentPage = pageNumber;
        self.tableConfig.numberOfPages = Math.ceil((self.tableConfig.records) * 1.0 / pageLength);
        if (self.tableConfig.numberOfPages <= 5) {
            for (let i = 1; i <= self.tableConfig.numberOfPages; i++) {
                self.tableConfig.pages.push({ pageNumber: i, pageLength: pageLength, active: pageNumber === i, disabled: false });
            }
        }
        else {
            if (pageNumber <= 4) {
                for (let i = 1; i <= 5; i++) {
                    self.tableConfig.pages.push({ pageNumber: i, pageLength: pageLength, active: pageNumber === i, disabled: false });
                }
                self.tableConfig.pages.push({ disabled: true });
                self.tableConfig.pages.push({ pageNumber: self.tableConfig.numberOfPages, pageLength: pageLength, active: pageNumber === self.tableConfig.numberOfPages });
            }
            else if (pageNumber + 2 <= self.tableConfig.numberOfPages) {
                self.tableConfig.pages.push({ pageNumber: 1, pageLength: pageLength, active: false, disabled: false });
                self.tableConfig.pages.push({ disabled: true });
                self.tableConfig.pages.push({ pageNumber: pageNumber - 1, pageLength: pageLength, active: false, disabled: false });
                self.tableConfig.pages.push({ pageNumber: pageNumber, pageLength: pageLength, active: true, disabled: false });
                self.tableConfig.pages.push({ pageNumber: pageNumber + 1, pageLength: pageLength, active: false, disabled: false });
                self.tableConfig.pages.push({ disabled: true });
                self.tableConfig.pages.push({ pageNumber: self.tableConfig.numberOfPages, pageLength: pageLength, active: false });
            }
            else {
                self.tableConfig.pages.push({ pageNumber: 1, pageLength: pageLength, active: pageNumber === 1, disabled: false });
                self.tableConfig.pages.push({ disabled: true });
                self.tableConfig.pages.push({ pageNumber: self.tableConfig.numberOfPages - 4, pageLength: pageLength, active: pageNumber === self.tableConfig.numberOfPages - 2, disabled: false });
                self.tableConfig.pages.push({ pageNumber: self.tableConfig.numberOfPages - 3, pageLength: pageLength, active: pageNumber === self.tableConfig.numberOfPages - 2, disabled: false });
                self.tableConfig.pages.push({ pageNumber: self.tableConfig.numberOfPages - 2, pageLength: pageLength, active: pageNumber === self.tableConfig.numberOfPages - 2, disabled: false });
                self.tableConfig.pages.push({ pageNumber: self.tableConfig.numberOfPages - 1, pageLength: pageLength, active: pageNumber === self.tableConfig.numberOfPages - 1, disabled: false });
                self.tableConfig.pages.push({ pageNumber: self.tableConfig.numberOfPages, pageLength: pageLength, active: pageNumber === self.tableConfig.numberOfPages, disabled: false });
            }
        }
    }
}

export { tableHandler };
