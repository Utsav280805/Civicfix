const express = require('express');
const app = express();
const bodyParser = require('body-parser');
const cors = require('cors');
const mongoose = require('mongoose');

// Set up middleware
app.use(bodyParser.json());
app.use(cors());

// MongoDB connection URI
const mongoURI = process.env.MONGO_URI || 'mongodb+srv://user1:admin123@civicfixdata.b83jc.mongodb.net/?retryWrites=true&w=majority&appName=CivicfixData';

// Import routes
const complaintRoutes = require('./complaintRoutes');

// Connect to MongoDB
mongoose.connect(mongoURI, { useNewUrlParser: true, useUnifiedTopology: true })
  .then(() => console.log('Connected to MongoDB'))
  .catch((err) => console.log('Error connecting to MongoDB: ', err));

// Use complaint routes
app.use('/api/complaints', complaintRoutes);

// Set up port for the server
const port = process.env.PORT || 5000;
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});
