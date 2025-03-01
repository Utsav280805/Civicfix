const express = require('express');
const mongoose = require('mongoose');
const dotenv = require('dotenv');
dotenv.config();

const app = express();

app.use(express.json());

async function main() {
    try {
        await mongoose.connect(process.env.MONGODB_URI, {
            // Add any necessary options here
        });
        console.log('Database Connected');
    } catch (err) {
        console.error('Database connection error:', err);
    }
}

const User = require('./models/login'); // Correct the path to the User model

app.post('/user', async (req, res) => {
    const user = new User(req.body);
    try {
        await user.save();
        res.status(201).send(user);
    } catch (err) {
        res.status(400).send(err);
    }
});

main();

app.get('/', (req, res) => {
    res.send('Welcome to the Task Manager API');
});

app.listen(3000, () => {
    console.log('Server is running on port 3000');
});
