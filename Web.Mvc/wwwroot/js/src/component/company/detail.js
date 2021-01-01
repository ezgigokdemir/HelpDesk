import Vue from 'vue';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import tr from 'vee-validate/dist/locale/tr';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import moment from 'moment';

Vue.use(VueTheMask);
Vue.use(VeeValidate);
Validator.localize('tr', tr);

var app = new Vue({
    el: '#app',
    components: {
        'message': message
    },

    data: {
        config: {
            messages: [],
        },
        company: company,
        history: []
    },

    methods: {

    },

    filters: {
        formatDate: function (date) {
            return moment(date).format('DD/MM/YYYY');
        }
    },

    computed: {

    }
});

