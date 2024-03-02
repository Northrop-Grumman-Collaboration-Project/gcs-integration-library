from dataclasses import dataclass
from datetime import datetime
from enum import Enum
from typing import Any

from Types.Geolocation import Coordinate

class Status(Enum):
    IN_USE = "In Use"
    STANDBY = "Standby"
    EMERGENCY= "Emergency"


@dataclass(repr=False)
class Telemetry:
    pitch: float
    yaw: float
    roll: float
    speed: float
    altitude: float
    batteryLife: float
    currentCoordinate: Coordinate
    lastUpdated: datetime
    
    # heading? (degrees NWSE of where vedhicle is heading)
    # ground/airspeed
    
    def to_dict(self) -> dict[str, Any]:
        obj = {
            'pitch': self.pitch,
            'yaw': self.yaw,
            'roll': self.roll,
            'speed': self.speed,
            'altitude': self.altitude,
            'batteryLife': self.batteryLife,
            'currentCoordinate': vars(self.currentCoordinate),
            'lastUpdated': self.lastUpdated.timestamp()
        }
        return obj





    
    