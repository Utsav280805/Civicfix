const express = require('express');
const router = express.Router();
const Complaint = require('./models/complaint');

// Route to add a new complaint, with a check for nearby complaints
router.post('/add-complaint', async (req, res) => {
    const { name, mobileNumber, description, latitude, longitude, category } = req.body;

    try {
        // Check if there are nearby complaints within 5 meters
        const nearbyComplaints = await Complaint.find({
            location: {
                $nearSphere: {
                    $geometry: { type: 'Point', coordinates: [longitude, latitude] },
                    $maxDistance: 30 // 5 meters radius
                }
            }
        });

        // If nearby complaints exist, return the result without adding a new one
        if (nearbyComplaints.length > 0) {
            return res.status(400).json({ message: "Nearby complaints already exist", complaints: nearbyComplaints });
        }

        // Create a new complaint
        const newComplaint = new Complaint({
            name,
            mobileNumber,
            description,
            latitude,
            longitude,
            category,
            status: 'pending',  // Set the initial status as 'pending'
            isApproved: false,   // Set approval status to false
            location: {
                type: 'Point',
                coordinates: [longitude, latitude]
            }
        });

        // Save the complaint to the database
        await newComplaint.save();

        res.status(201).json({
            message: "Complaint added successfully",
            complaint: newComplaint
        });
    } catch (err) {
        console.error(err);
        res.status(500).json({ message: "Server error", error: err });
    }
});

// Route to check if any nearby complaints exist (for debugging or checking manually)
router.post('/check-nearby', async (req, res) => {
    const { latitude, longitude } = req.body;

    try {
        // Check for nearby complaints within 5 meters
        const nearbyComplaints = await Complaint.find({
            location: {
                $nearSphere: {
                    $geometry: { type: 'Point', coordinates: [longitude, latitude] },
                    $maxDistance: 30  // 5 meters radius
                }
            }
        });

        if (nearbyComplaints.length > 0) {
            return res.status(200).json({
                message: "Nearby complaints found",
                complaints: nearbyComplaints
            });
        } else {
            return res.status(200).json({
                message: "No nearby complaints found"
            });
        }
    } catch (err) {
        console.error(err);
        return res.status(500).json({ message: "Server error", error: err });
    }
});

module.exports = router;
