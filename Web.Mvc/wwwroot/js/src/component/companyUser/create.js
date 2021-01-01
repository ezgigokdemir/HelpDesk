import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import tr from 'vee-validate/dist/locale/tr';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import _ from 'lodash';
import 'vue-multiselect/dist/vue-multiselect.min.css';
import toastr from 'toastr';
import '../../../../../node_modules/toastr/build/toastr.css';

toastr.options.progressBar = true;
toastr.options.positionClass = 'toast-bottom-right';

Vue.use(VueTheMask);
Vue.use(VeeValidate);

Validator.localize('tr', tr);

VeeValidate.Validator.extend('verify_password', {
    getMessage: field => `Şifre: 1 büyük harf, 1 küçük harf, 1 rakam ve bir özel karakter (Örn. , . _ & ? vb.)`,
    validate: value => {
        var strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*(),.?:{}|<>\*])(?=.{8,})");
        return strongRegex.test(value);
    }
});

var app = new Vue({
    el: '#app',
    components: {
        'message': message
    },
    data: {
        config: {
            messages: [],
            keyFilter: []
        },
        companyUserViewModel: model,
        companies: companies,
        currentRoles: currentRoles,
        companySelect: {},

        selections: [{ Name: 'Firma Admin', IsChecked: false }, { Name: 'Firma Kullanıcısı', IsChecked: false }]
    },
    methods: {
        onSubmit: function () {
            console.log("onsubmit çalıştı");
            var self = this;
            this.$validator.validate().then(result => {
                if (!result) {
                    swal.fire({
                        type: '',
                        title: '',
                        text: 'Lütfen form bilgilerini kontrol edin!',
                        animation: false
                    });
                } else {
                    this.submit();
                }
            });
        },

        submit: function () {
            let self = this;
            console.log("submit çalıştı");
            self.companyUserViewModel.UserDto.CompanyId = self.companySelect.Id;
            self.companyUserViewModel.CompanyId = self.companySelect.Id;
            //self.companyUserViewModel.Company = self.filteredData;
            // console.log(self.companyUserViewModel);
            // self.companyUserViewModel.UserDto.IsCompanyUserLimitExceeded = true;
            console.log("self.companyUserViewModel", self.companyUserViewModel);
            axios.post('/identity/companyuser/createasync', self.companyUserViewModel)
                .then(function (response) {

                    console.log("kayıt başarılı");
                    self.companyUserViewModel.UserDto = {};
                    self.companyUserViewModel.IdentityUserId = '';
                    self.companyUserViewModel.Password = '';
                    //self.role.Selected = false;

                    self.config.messages.push({ type: 'success', content: 'Kayıt işlemi başarıyla tamamlandı! Listeye dönmek için <a href="/identity/companyuser/index" class="btn btn-info" target="_self">tıklayınız</a>' });
                    window.scrollTo({
                        top: 0,
                        left: 0,
                        behavior: "smooth"
                    });
                }).catch(function (error) {
                    console.log("kayıt başarısız");
                    if (error.response.status == 422) {
                        swal.fire({
                            html: 'Firma Kullanıcı kapasitesi dolmuştur',
                            showCloseButton: true,
                            confirmButtonText: 'Tamam'
                        })
                        // self.companyUserViewModel.UserDto.IsCompanyUserLimitExceeded = false;
                    }
                    else {
                        self.config.messages.push({ type: 'warning', content: 'Kayıt işlemi gerçekleştirilemedi, aynı eposta adresi ile daha önce hesap açılmadığından emin olun.' });
                        window.scrollTo({
                            top: 0,
                            left: 0,
                            behavior: "smooth"
                        });
                        console.log(error);
                    }

                });
        },


        showIsRoleAdmin: function () {

            for (var i = 0; i < this.currentRoles.length; i++) {
                if (this.currentRoles[i] === 'Admin')
                    document.getElementById("companySelection").style.display = "block";
            }
        },
    },

    created: function () {
        this.showIsRoleAdmin();

    },

    computed: {

        filteredData: function () {
            let data = Object.entries(this.companies);
            console.log("baseData: ", data);
            if (this.config.keyFilter.length > 0) {
                //console.log("keyfilter", this.config.keyFilter);
                //console.log("filter 1", (data[1])[1]);
                var newData = (data[1])[1];
                newData = newData.filter(val => this.config.keyFilter.indexOf(val.CompanyName) > -1);
            }
            console.log("filtereddata", newData);
            return newData;
        },

        companyList: function () {
            return this.companies.data.map(val => val.CompanyName);
        },


    },
    mounted: function () {
        // this.$nextTick(this.companyCapacityAlert);
    },
    filters: {
    }
});
