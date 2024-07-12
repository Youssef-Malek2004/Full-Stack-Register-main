import React, { useState, useEffect } from "react";
import backgroundImage from "../Components/background2.png";
import axios from "axios";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

function CreatingForm() {
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

  const [governates, setGovernates] = useState([]);
  const [cities, setCities] = useState([]);
  const [confirmationPopup, setConfirmationPopup] = useState(false);
  const [correlationID, setCorrelationID] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get("http://localhost:5000/GetLookUps");
        setGovernates(response.data.governates);
        setCities(response.data.cities);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  const handleAddAddress = () => {
    setFormData({
      ...formData,
      addressList: [
        ...formData.addressList,
        {
          governate: "",
          city: "",
          street: "",
          buildingNumber: "",
          flatNumber: "",
        },
      ],
    });
  };

  const handleDateChange = (date) => {
    const trimmedDate = date.toISOString().split("T")[0];
    setFormData({ ...formData, birthDate: trimmedDate });
  };

  const handleAddressChange = (index, event) => {
    const newAddressList = [...formData.addressList];
    newAddressList[index][event.target.name] = event.target.value;
    setFormData({ ...formData, addressList: newAddressList });
  };

  const handleRegisterClick = async (e) => {
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
      monitorStatus(result.correlationID);
    } catch (error) {
      console.error("Error:", error);
      alert(`Error: ${error.message}`);
    }
  };

  const monitorStatus = async (correlationID) => {
    try {
      // Make GET request to check status using correlationID
      const getResponse = await axios.get(
        `http://localhost:5142/register?correlationId=${correlationID}`
      );

      const responseDataString = JSON.stringify(getResponse.data, null, 2);
      // setStatusResponse(getResponse.data.status);

      // // Check if the status is "Task is pending"
      if (responseDataString.includes("Task is still pending")) {
        // Repeat after 1 second (you can adjust the delay as needed)
        alert(
          "Task is still pending, You will be notified when the task is completed."
        );
        setTimeout(() => monitorStatus(correlationID), 1000);
      } else {
        // Handle other statuses or completion of task
        console.log("Task completed:", getResponse.data);
        alert(JSON.stringify(getResponse.data, null, 2));
      }
    } catch (error) {
      console.error("Error checking status:", error);
    }
  };

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    console.log(formData);
  };

  return (
    <>
      <div
        className="min-h-screen  bg-cover bg-center justify-center"
        style={{ backgroundImage: `url(${backgroundImage})` }}
      >
        <div className="h-screen ">
          <div className="min-h-screen items-center w-1/2 justify-center  my-auto flex flex-col">
            <div
              className=" py-8 px-4 sm:px-6 lg:px-8"
              style={{
                background: "linear-gradient(to right, #9B1B59, #6b2d98)",
              }}
            >
              <div className="max-w-md w-full space-y-8 ">
                <h2 className="mt-6 text-center text-3xl font-extrabold text-white">
                  Register
                </h2>
                <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <label htmlFor="firstName" className="sr-only">
                        First Name
                      </label>
                      <input
                        id="firstName"
                        name="firstName"
                        type="text"
                        autoComplete="given-name"
                        required
                        className="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-black focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                        placeholder="First Name"
                        value={formData.firstName}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label htmlFor="middleName" className="sr-only">
                        Middle Name
                      </label>
                      <input
                        id="middleName"
                        name="middleName"
                        type="text"
                        autoComplete="given-name"
                        required
                        className="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-black focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                        placeholder="Middle Name"
                        value={formData.middleName}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label htmlFor="lastName" className="sr-only">
                        Last Name
                      </label>
                      <input
                        id="lastName"
                        name="lastName"
                        type="text"
                        autoComplete="family-name"
                        required
                        className="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-black focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                        placeholder="Last Name"
                        value={formData.lastName}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label htmlFor="email" className="sr-only">
                        Email address
                      </label>
                      <input
                        id="email"
                        name="email"
                        type="email"
                        autoComplete="email"
                        required
                        className="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-black focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                        placeholder="Email address"
                        value={formData.email}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label htmlFor="mobileNumber" className="sr-only">
                        Mobile Number
                      </label>
                      <input
                        id="mobileNumber"
                        name="mobileNumber"
                        type="text"
                        autoComplete="tel"
                        required
                        className="rounded-md align-top w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-black focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        placeholder="Mobile Number"
                        value={formData.mobileNumber}
                        onChange={handleChange}
                      />
                    </div>
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
                    <div className="mt">
                      <button
                        type="button"
                        className="w-full py-2 px-4  bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 rounded-md text-sm text-white font-medium"
                        onClick={() => setConfirmationPopup(true)}
                      >
                        Pick Addresses
                      </button>
                    </div>
                  </div>
                  <div>
                    <button
                      type="submit"
                      onClick={handleRegisterClick}
                      className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                    >
                      Register
                    </button>
                  </div>
                </form>
              </div>
            </div>
            <footer className="text-center text-gray-500 text-sm py-4">
              All rights reserved &copy; {new Date().getFullYear()}{" "}
            </footer>
          </div>
        </div>
        {/* Address Popup */}
        {confirmationPopup && (
          <div className="fixed inset-0 flex items-center justify-center z-50 backdrop-blur confirm-dialog">
            <div className="relative px-4 min-h-screen md:flex md:items-center md:justify-center">
              <div className="opacity-25 w-full h-full absolute z-10 inset-0"></div>
              <div className="bg-white rounded-lg md:max-w-md md:mx-auto p-4 fixed inset-x-0 bottom-0 z-50 mb-4 mx-4 md:relative shadow-lg">
                <div className="md:flex items-center">
                  <div className="mt-4 md:mt-0 md:ml-6 text-center md:text-left">
                    <p className="font-bold">Add Addresses</p>
                    <form onSubmit={handleSubmit}>
                      {formData.addressList.map((address, index) => (
                        <div key={index} className="mt-2">
                          <select
                            name="governate"
                            value={address.governate}
                            onChange={(event) =>
                              handleAddressChange(index, event)
                            }
                            className="mb-2 p-2 border"
                          >
                            <option value="">Select Governate</option>
                            {governates.map((gov) => (
                              <option
                                key={gov.governateID}
                                value={gov.governateID}
                              >
                                {gov.governateID}
                              </option>
                            ))}
                          </select>
                          <select
                            name="city"
                            value={address.city}
                            onChange={(event) =>
                              handleAddressChange(index, event)
                            }
                            className="mb-2 p-2 border"
                          >
                            <option value="">Select City</option>
                            {cities.map((city) => (
                              <option key={city.cityID} value={city.cityID}>
                                {city.cityID}
                              </option>
                            ))}
                          </select>
                          <input
                            type="text"
                            name="street"
                            value={address.street}
                            onChange={(event) =>
                              handleAddressChange(index, event)
                            }
                            placeholder="Street"
                            className="block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm mt-2"
                          />
                          <input
                            type="text"
                            name="buildingNumber"
                            value={address.buildingNumber}
                            onChange={(event) =>
                              handleAddressChange(index, event)
                            }
                            placeholder="Building Number"
                            className="block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm mt-2"
                          />
                          <input
                            type="number"
                            name="flatNumber"
                            value={address.flatNumber}
                            onChange={(event) =>
                              handleAddressChange(index, event)
                            }
                            placeholder="Flat Number"
                            className="block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm mt-2"
                          />
                        </div>
                      ))}
                      <button
                        type="button"
                        onClick={handleAddAddress}
                        className="mt-2 w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
                      >
                        Add Another Address
                      </button>
                      <div className="flex justify-end items-end mt-3">
                        <button
                          type="submit"
                          className="flex justify-end items-end py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500 ml-5"
                        >
                          Submit
                        </button>
                        <button
                          type="button"
                          className="flex justify-end items-end py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 ml-5"
                          onClick={() => setConfirmationPopup(false)}
                        >
                          Close
                        </button>
                      </div>
                    </form>
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </>
  );
}

export default CreatingForm;
