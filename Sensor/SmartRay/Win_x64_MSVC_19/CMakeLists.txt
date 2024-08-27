cmake_minimum_required(VERSION 3.10)

project(SR_API)

message(STATUS "  >> ${PROJECT_NAME} >>")

include(../cmake/import_library.cmake)

ImportShared(
	SR_API
	"${CMAKE_CURRENT_SOURCE_DIR}/lib"
	""
	"-x64"
	"${CMAKE_CURRENT_SOURCE_DIR}/bin"
	""
	"-x64"
	"${CMAKE_CURRENT_SOURCE_DIR}/include/sr_api"
)

message(STATUS "  << ${PROJECT_NAME} <<")


