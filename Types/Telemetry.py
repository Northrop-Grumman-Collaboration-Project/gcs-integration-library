from dataclasses import dataclass
from datetime import datetime
from enum import Enum
from typing import Dict

from Types.Geolocation import Coordinate

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
    
    def to_dict(self) -> Dict[str, Any]:
        obj = {
            'pitch': self.pitch,
            'yaw': self.yaw,
            'roll':self.roll,
            'speed':self.speed,
            'altitude': self.altitude,
            'batteryLife':self.batteryLife,
            'currentCoordinate': vars(self.currentCoordinate),
            'lastUpdated': self.lastUpdated.timestamp(),
        }
        return obj





    
    
