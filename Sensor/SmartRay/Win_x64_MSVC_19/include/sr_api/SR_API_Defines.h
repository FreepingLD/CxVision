/******************************************************************************
 *                          SmartRay API Defines                              *
 *----------------------------------------------------------------------------*
 * Copyright (c) SmartRay GmbH 2018.  All Rights Reserved.		              *
 *----------------------------------------------------------------------------*
 * Title    SR_API_Defines.h                                                  *
 * Version	SR_API_VERSION                                                    *
 ******************************************************************************/

/**
 * @file SR_API_Defines.h
 * @author SmartRay GmbH
 * @date 2018
 * @brief SmartRay macro definitions
 * @see http://www.smartray.com
 */

#ifndef SR_API_DEFINESh
#define SR_API_DEFINESh

#ifdef sr_api_STATIC
	#define SR_API_EXPORT
#else // not sr_api_STATIC
	#ifdef sr_api_SHARED
		#define SR_API_EXPORT __declspec(dllexport)
	#else // not sr_api_SHARED
		#define SR_API_EXPORT __declspec(dllimport)
	#endif // sr_api_SHARED
#endif // sr_api_STATIC

#if _WIN32 || _WIN64
#	if _WIN64
#		define INSTANCES 64                                /**< Maximum sensors supported by API.*/
#	else
#		define INSTANCES 16                                /**< Maximum sensors supported by API.*/
#	endif
#else
#	define INSTANCES 64                                /**< Maximum sensors supported by API.*/
#endif

#  ifdef __cplusplus
typedef bool bool8_t;                               /**< Boolean type.*/
#  else
typedef uint8_t bool8_t;                            /**< Boolean type.*/
#  endif

#define ENABLE			1                           /**< Enable.*/
#define DISABLE			0                           /**< Disable.*/

/** \name Default values: */
/**@{*/
#define DEFAULT_IP_ADR 		"192.168.178.200"       /**< Default sensor IP address.*/
#define DEFAULT_PORT_NUM 	40                      /**< Default sensor port number.*/
/**@}*/

/** \name Laser mode: */
/**@{*/
#define LASER_PULSED      0                         /**< Laser mode: pulsed.*/
#define LASER_CONTINOUS   1                         /**< Laser mode: continous.*/
/**@}*/

/** \name Laser power: */
/**@{*/
#define LASER_EXT_ON      1                         /**< Laser power: ext. on.*/
#define LASER_SW_ONLY     0                         /**< Laser power: SW only.*/
/**@}*/

/** \name Laser enable: */
/**@{*/
#define LASER_ENABLE      1                         /**< Laser enable.*/
#define LASER_DISABLE     0                         /**< Laser disable.*/
#define LASER_DONOTCHANGE 99                        /**< Laser do not change.*/
/**@}*/

/** \name SmartXact modes: */
/**@{*/
#define SR_API_SMARTXACT_MODE_DEFAULT        0      /**< Mode default*/
#define SR_API_SMARTXACT_MODE_METROLOGY      1      /**< Mode metrology*/
/**@}*/

/** \name IO related: */
/**@{*/
#define IO_HIGH	1                                   /**< IO high.*/
#define IO_LOW	0                                   /**< IO low.*/
/**@}*/

/** \name Misc. sizes and timer values: */
/**@{*/
#define HEADERSIZE_USER	9                           /**< Header size user.*/
#define ALIVE_TIMEOUT	10.0                        /**< Alive timeout (for sensor connection alive signal). Unit: sec.*/
#define FILENAMESIZE	260                         /**< Filename size.*/
#define IPADRSIZE		4                           /**< IP address size.*/
#define MACSIZE			24                          /**< MAC address size.*/
#define SERIALSIZE		24                          /**< Serial size.*/
#define CAMVERSIONSIZE	4096                        /**< Camera version size.*/
#define PARTNUMBERSIZE 	16                          /**< Part number size.*/
/**@}*/

/** \name Maximum callback functions that can be registered: */
/**@{*/
#define MAXNUM_SRCB_LIVEIMAGE 	8	                /**< Maximum Live-Image callback functions that can be registered.*/
#define MAXNUM_SRCB_PIL 		8	                /**< Maximum PIL callback functions that can be registered.*/
#define MAXNUM_SRCB_ZIL 		8	                /**< Maximum ZIL callback functions that can be registered.*/
/**@}*/

/** \name Data buffers sizes: */
/**@{*/
#define EXTMODE_BUFSIZE 	10000000                /**< Buffer size.*/
#define EXTMODE_EXTDATASIZE 10000                   /**< Ext. data size.*/
/**@}*/

/** \name Invalid point cloud codes corresponding to invalid 3D profile points: */
/**@{*/
#define NONWORLDMARK  -999990.0                     /**< Invalid 3D profile point: non world mark.*/
#define NONWORLDVALUE -999997.0                     /**< Invalid 3D profile point: non world value.*/
#define NONLUTVALUE   -999998.0                     /**< Invalid 3D profile point: non LUT value.*/
#define BADWORLDVALUE -999999.0                     /**< Invalid 3D profile point: bad world value.*/
/**@}*/

#endif // SR_API_DEFINESh
