const mongoose = require('mongoose');

const userSchema = new mongoose.Schema({
    username: {
        type: String,
        required: true,
    },
    password: {
        type: Number,
        required: true,
        minlength: 4,
    },
    mobileNumber: {
        type: Number,
        required: true,
        unique: true,
        
    }
});

const User = mongoose.model('User', userSchema);

module.exports = User;


