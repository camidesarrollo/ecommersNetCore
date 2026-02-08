/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "../Views/**/*.cshtml",
        "../Pages/**/*.cshtml",
        "../../../wwwroot/js/**/*.js",
        "../../../wwwroot/**/*.html"
    ],
    theme: {
        extend: {},
    },
    //plugins: [],
    plugins: [
        require('../../../wwwroot/js/application/plugins/tailwind-ecommerce-plugin.js'),
    ],
};