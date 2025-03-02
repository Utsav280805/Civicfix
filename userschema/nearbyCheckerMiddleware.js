const Complaint = require('./models/complaint');

// Middleware to check if there are nearby complaints
const checkNearbyComplaints = async (req, res, next) => {
    const { latitude, longitude } = req.body;

    try {
        // Check if there are nearby complaints within 50 meters
        const nearbyComplaints = await Complaint.find({
            location: {
                $nearSphere: {
                    $geometry: { type: 'Point', coordinates: [longitude, latitude] },
                    $maxDistance: 50  // 50 meters radius
                }
            }
        });

        // If nearby complaints exist, return an error response
        if (nearbyComplaints.length > 0) {
            return res.status(400).json({
                message: "Nearby complaints already exist",
                complaints: nearbyComplaints
            });
        }

        // If no nearby complaints, continue to the next middleware/route handler
        next();
    } catch (err) {
        console.error(err);
        res.status(500).json({ message: "Server error", error: err });
    }
};

module.exports = checkNearbyComplaints;
