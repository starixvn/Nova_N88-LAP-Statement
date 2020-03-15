var commonRules = [
   {
       test: require.resolve("jquery"),
       use: [{
           loader: "expose-loader",
           options: "jQuery"
       }, {
           loader: "expose-loader",
           options: "$"
       }]
   }
];

module.exports = {
    commonRules: commonRules
};