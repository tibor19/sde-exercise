function (user, context, callback) {
    if (context.clientID !== '{CLIENT_ID}') {
        return callback(null, user, context);
    }
    var adminRole = 'Admin';
    var userRole = 'User';
    var adminUsers = ['email1@somerandomdomain.io', 'email1@someotherrandomdomain.io'];
    var adminDomain = '@somedomain.com';
    user.app_metadata = user.app_metadata || {};

    // You can add a Role based on what you want. Please note that the role names are case sensitive.
    // In this case I check if the user is in the adminUsers array or belongs to a certain domain 
    var addRolesToUser = function (user, cb) {
        if (adminUsers.indexOf(user.email) !== -1 || user.email.endsWith(adminDomain)) {
            cb(null, [adminRole, userRole]);
        } else {
            cb(null, [userRole]);
        }
    };

    addRolesToUser(user, function (err, roles) {
        if (err) {
            callback(err);
        } else {
            user.app_metadata.roles = roles;
            auth0.users.updateAppMetadata(user.user_id, user.app_metadata)
                .then(function () {
                    callback(null, user, context);
                })
                .catch(function (err) {
                    callback(err);
                });
        }
    });
}