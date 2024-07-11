import React, { useState, useEffect } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const MyForm = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    middleName: "",
    lastName: "",
    birthDate: null,
    mobileNumber: "",
    email: "",
    addressList: [
      {
        governate: "",
        city: "",
        street: "",
        buildingNumber: "",
        flatNumber: "",
      },
    ],
  });

  const [governateOptions, setGovernateOptions] = useState([]);
  const [cityOptions, setCityOptions] = useState([]);

  useEffect(() => {
    // Simulating fetching governate options from API
    const fetchGovernateOptions = async () => {
      try {
        // Replace with actual fetch call to your API endpoint
        // const response = await fetch("/api/governates");
        // const data = await response.json();
        const data = [
          { id: 1, name: "Cairo" },
          { id: 2, name: "Alexandria" },
          { id: 3, name: "Giza" },
        ];
        setGovernateOptions(data);
      } catch (error) {
        console.error("Error fetching governates:", error);
      }
    };

    fetchGovernateOptions();
  }, []);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleDateChange = (date) => {
    setFormData({ ...formData, birthDate: date });
  };

  const handleAddressChange = (index, e) => {
    const { name, value } = e.target;
    const updatedAddressList = [...formData.addressList];
    updatedAddressList[index][name] = value;
    setFormData({ ...formData, addressList: updatedAddressList });
  };

  const handleGovernateChange = (index, e) => {
    const { value } = e.target;
    const updatedAddressList = [...formData.addressList];
    updatedAddressList[index].governate = value;
    setFormData({ ...formData, addressList: updatedAddressList });

    // Simulating fetching city options based on selected governate
    const fetchCityOptions = async () => {
      try {
        // Replace with actual fetch call to your API endpoint
        // const response = await fetch(`/api/cities?governate=${value}`);
        // const data = await response.json();
        const data = [
          { id: 101, name: "Nasr City" },
          { id: 102, name: "Maadi" },
          { id: 103, name: "Dokki" },
        ];
        setCityOptions(data);
      } catch (error) {
        console.error(`Error fetching cities for governate ${value}:`, error);
      }
    };

    fetchCityOptions();
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const jsonInput = JSON.stringify(formData, null, 2);
      console.log("JSON Input:", jsonInput);

      // Make POST request to API
      const res = await fetch("http://localhost:5142/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: jsonInput,
      });

      if (!res.ok) {
        throw new Error(`HTTP error! Status: ${res.status}`);
      }

      const result = await res.json();
      console.log("API Response:", result);
      alert(JSON.stringify(result, null, 2));
    } catch (error) {
      console.error("Error:", error);
      alert(`Error: ${error.message}`);
    }
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-bold mb-4">Registration Form</h1>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
          {/* Left Column */}
          <div>
            {/* First Name */}
            <div>
              <label
                htmlFor="firstName"
                className="block text-sm font-medium text-gray-700"
              >
                First Name *
              </label>
              <input
                type="text"
                id="firstName"
                name="firstName"
                value={formData.firstName}
                onChange={handleInputChange}
                className="w-full p-2 border rounded"
                required
              />
            </div>

            {/* Middle Name */}
            <div>
              <label
                htmlFor="middleName"
                className="block text-sm font-medium text-gray-700"
              >
                Middle Name
              </label>
              <input
                type="text"
                id="middleName"
                name="middleName"
                value={formData.middleName}
                onChange={handleInputChange}
                className="w-full p-2 border rounded"
              />
            </div>

            {/* Last Name */}
            <div>
              <label
                htmlFor="lastName"
                className="block text-sm font-medium text-gray-700"
              >
                Last Name *
              </label>
              <input
                type="text"
                id="lastName"
                name="lastName"
                value={formData.lastName}
                onChange={handleInputChange}
                className="w-full p-2 border rounded"
                required
              />
            </div>
          </div>

          {/* Right Column */}
          <div>
            {/* Birth Date */}
            <div>
              <label
                htmlFor="birthDate"
                className="block text-sm font-medium text-gray-700"
              >
                Birth Date *
              </label>
              <DatePicker
                id="birthDate"
                selected={formData.birthDate}
                onChange={handleDateChange}
                className="w-full p-2 border rounded"
                required
                dateFormat="yyyy-MM-dd"
              />
            </div>

            {/* Mobile Number */}
            <div>
              <label
                htmlFor="mobileNumber"
                className="block text-sm font-medium text-gray-700"
              >
                Mobile Number *
              </label>
              <input
                type="text"
                id="mobileNumber"
                name="mobileNumber"
                value={formData.mobileNumber}
                onChange={handleInputChange}
                className="w-full p-2 border rounded"
                required
              />
            </div>

            {/* Email */}
            <div>
              <label
                htmlFor="email"
                className="block text-sm font-medium text-gray-700"
              >
                Email *
              </label>
              <input
                type="email"
                id="email"
                name="email"
                value={formData.email}
                onChange={handleInputChange}
                className="w-full p-2 border rounded"
                required
              />
            </div>
          </div>
        </div>

        {/* Address List */}
        <div>
          <h2 className="text-xl font-bold mt-4 mb-2">Address List</h2>
          {formData.addressList.map((address, index) => (
            <div key={index} className="space-y-2">
              {/* Governate Dropdown */}
              <div>
                <label
                  htmlFor={`governate-${index}`}
                  className="block text-sm font-medium text-gray-700"
                >
                  Governate *
                </label>
                <select
                  id={`governate-${index}`}
                  name={`governate-${index}`}
                  value={address.governate}
                  onChange={(e) => handleGovernateChange(index, e)}
                  className="w-full p-2 border rounded"
                  required
                >
                  <option value="">Select Governate</option>
                  {governateOptions.map((gov) => (
                    <option key={gov.id} value={gov.name}>
                      {gov.name}
                    </option>
                  ))}
                </select>
              </div>

              {/* City Dropdown */}
              <div>
                <label
                  htmlFor={`city-${index}`}
                  className="block text-sm font-medium text-gray-700"
                >
                  City *
                </label>
                <select
                  id={`city-${index}`}
                  name={`city-${index}`}
                  value={address.city}
                  onChange={(e) => handleAddressChange(index, e)}
                  className="w-full p-2 border rounded"
                  required
                >
                  <option value="">Select City</option>
                  {cityOptions.map((city) => (
                    <option key={city.id} value={city.name}>
                      {city.name}
                    </option>
                  ))}
                </select>
              </div>

              {/* Street */}
              <div>
                <label
                  htmlFor={`street-${index}`}
                  className="block text-sm font-medium text-gray-700"
                >
                  Street *
                </label>
                <input
                  type="text"
                  id={`street-${index}`}
                  name={`street-${index}`}
                  value={address.street}
                  onChange={(e) => handleAddressChange(index, e)}
                  className="w-full p-2 border rounded"
                  required
                  maxLength={200}
                />
              </div>

              {/* Building Number */}
              <div>
                <label
                  htmlFor={`buildingNumber-${index}`}
                  className="block text-sm font-medium text-gray-700"
                >
                  Building Number *
                </label>
                <input
                  type="text"
                  id={`buildingNumber-${index}`}
                  name={`buildingNumber-${index}`}
                  value={address.buildingNumber}
                  onChange={(e) => handleAddressChange(index, e)}
                  className="w-full p-2 border rounded"
                  required
                />
              </div>

              {/* Flat Number */}
              <div>
                <label
                  htmlFor={`flatNumber-${index}`}
                  className="block text-sm font-medium text-gray-700"
                >
                  Flat Number *
                </label>
                <input
                  type="number"
                  id={`flatNumber-${index}`}
                  name={`flatNumber-${index}`}
                  value={address.flatNumber}
                  onChange={(e) => handleAddressChange(index, e)}
                  className="w-full p-2 border rounded"
                  required
                />
              </div>
            </div>
          ))}
        </div>

        {/* Submit Button */}
        <button
          type="submit"
          className="mt-4 p-2 bg-blue-500 text-white rounded"
        >
          Submit
        </button>
      </form>
    </div>
  );
};

export default MyForm;
