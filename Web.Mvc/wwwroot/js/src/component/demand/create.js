﻿import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import tr from 'vee-validate/dist/locale/tr';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import moment from 'moment';
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
            showInfo: false
        },
        demandDto: {},
        urgencies: urgencies
    },
    methods: {
        onSubmit: function () {
            this.$validator.validate().then(result => {
                if (!result) {
                    swal({
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
            
            axios.post('/demand/createdemandasync', self.demandDto).then(function (response) {

                console.log("kayıt başarılı");
                self.demandDto = {};

                self.config.messages.push({ type: 'success', content: 'Kayıt işlemi başarıyla tamamlandı!' });
                window.scrollTo({
                    top: 0,
                    left: 0,
                    behavior: "smooth"
                });
            }).catch(function (error) {
                console.log("kayıt başarısız");
                self.config.messages.push({ type: 'warning', content: 'Kayıt işlemi gerçekleştirilemedi!' });
                window.scrollTo({
                    top: 0,
                    left: 0,
                    behavior: "smooth"
                });
                console.log(error);
            });
        }
    },

    created: function () {

    },

    computed: {
    },

    filters: {
        formatDate: function (value) {
            if (!value) return '';
            return moment(String(value)).format('DD/MM/YYYY');
        }
    }
});
