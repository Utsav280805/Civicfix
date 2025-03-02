const mongoose = require('mongoose');

const complaintSchema = new mongoose.Schema({
    name: { type: String, required: true },
    mobileNumber: { type: Number, required: true },
    description: { type: String, required: true },
    image: { type: Buffer, required: false },
    latitude: { type: Number, required: true },
    longitude: { type: Number, required: true },
    category: { type: String, required: true },
    status: { type: String, enum: ['pending', 'approved', 'rejected'], default: 'pending' },
    isApproved: { type: Boolean, default: false },
    location: { type: { type: String, enum: ['Point'], required:false }, coordinates: [Number] }, // Geospatial data
});

complaintSchema.index({ location: '2dsphere' });  // Geospatial index

complaintSchema.pre('save', function (next) {
    if (this.isApproved) {
        this.status = 'approved';
    }
    this.location = { type: 'Point', coordinates: [this.longitude, this.latitude] }; // Add location field
    next();
});

const Complaint = mongoose.model('Complaint', complaintSchema);

module.exports = Complaint;